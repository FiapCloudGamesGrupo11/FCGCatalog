using System.Text.Json;
using Amazon;
using FiapCloudGames.Domain.Entity;
using FiapCloudGames.Domain.Interfaces;
using FiapCloudGames.Domain.Search;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenSearch.Net;
using OpenSearch.Net.Auth.AwsSigV4;

namespace FiapCloudGames.Infrastructure.Search;

public sealed class OpenSearchGameSearchService : IGameSearchService
{
    private readonly OpenSearchSettings _settings;
    private readonly ILogger<OpenSearchGameSearchService> _logger;
    private readonly OpenSearchLowLevelClient? _client;
    private readonly SemaphoreSlim _indexLock = new(1, 1);
    private bool _indexReady;

    public OpenSearchGameSearchService(
        IOptions<OpenSearchSettings> settings,
        ILogger<OpenSearchGameSearchService> logger)
    {
        _settings = settings.Value;
        _logger = logger;

        if (Uri.TryCreate(_settings.Endpoint, UriKind.Absolute, out var endpoint))
        {
            var configuration = _settings.UseAwsSigV4
                ? new ConnectionConfiguration(
                    endpoint,
                    new AwsSigV4HttpConnection(RegionEndpoint.GetBySystemName(_settings.AwsRegion)))
                : new ConnectionConfiguration(endpoint);

            _client = new OpenSearchLowLevelClient(configuration);
        }
    }

    public async Task<IReadOnlyCollection<GameSearchResult>> SearchAsync(
        string query,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        EnsureConfigured();
        await EnsureIndexAsync(cancellationToken);

        var body = new
        {
            from = (page - 1) * pageSize,
            size = pageSize,
            query = new
            {
                multi_match = new
                {
                    query,
                    fields = new[] { "name^5", "tags^3", "genres^2", "category^2", "developer", "publisher", "description" },
                    fuzziness = "AUTO",
                    type = "best_fields"
                }
            }
        };

        var response = await _client!.SearchAsync<StringResponse>(
            _settings.IndexName,
            PostData.Serializable(body),
            ctx: cancellationToken);

        EnsureSuccess(response, "search");
        return ParseResults(response.Body);
    }

    public async Task IndexAsync(GameFullData game, CancellationToken cancellationToken = default)
    {
        if (_client is null)
        {
            _logger.LogInformation("OpenSearch is not configured; game {GameId} was not indexed.", game.Id);
            return;
        }

        try
        {
            await EnsureIndexAsync(cancellationToken);
            var response = await _client.IndexAsync<StringResponse>(
                _settings.IndexName,
                game.Id.ToString(),
                PostData.Serializable(ToDocument(game)),
                ctx: cancellationToken);
            EnsureSuccess(response, "index game");
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Game {GameId} was saved in MongoDB but could not be indexed in OpenSearch.", game.Id);
        }
    }

    public async Task<int> ReindexAsync(IEnumerable<GameFullData> games, CancellationToken cancellationToken = default)
    {
        EnsureConfigured();
        await EnsureIndexAsync(cancellationToken);

        var count = 0;
        foreach (var game in games)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var response = await _client!.IndexAsync<StringResponse>(
                _settings.IndexName,
                game.Id.ToString(),
                PostData.Serializable(ToDocument(game)),
                ctx: cancellationToken);
            EnsureSuccess(response, "reindex game");
            count++;
        }

        return count;
    }

    private async Task EnsureIndexAsync(CancellationToken cancellationToken)
    {
        if (_indexReady) return;

        await _indexLock.WaitAsync(cancellationToken);
        try
        {
            if (_indexReady) return;

            var exists = await _client!.Indices.ExistsAsync<StringResponse>(_settings.IndexName, ctx: cancellationToken);
            if (exists.HttpStatusCode == 404)
            {
                var mapping = new
                {
                    settings = new { number_of_shards = 1, number_of_replicas = 1 },
                    mappings = new
                    {
                        properties = new
                        {
                            id = new { type = "keyword" },
                            name = new { type = "text" },
                            description = new { type = "text" },
                            category = new { type = "text", fields = new { keyword = new { type = "keyword" } } },
                            developer = new { type = "text" },
                            publisher = new { type = "text" },
                            genres = new { type = "text" },
                            platforms = new { type = "keyword" },
                            tags = new { type = "text" },
                            price = new { type = "double" },
                            releaseDate = new { type = "date" },
                            rating = new { type = "keyword" }
                        }
                    }
                };

                var created = await _client.Indices.CreateAsync<StringResponse>(
                    _settings.IndexName,
                    PostData.Serializable(mapping),
                    ctx: cancellationToken);
                EnsureSuccess(created, "create index");
            }
            else
            {
                EnsureSuccess(exists, "check index");
            }

            _indexReady = true;
        }
        finally
        {
            _indexLock.Release();
        }
    }

    private void EnsureConfigured()
    {
        if (_client is null)
            throw new InvalidOperationException("OpenSearch is not configured. Define OpenSearch__Endpoint.");
    }

    private static object ToDocument(GameFullData game) => new
    {
        id = game.Id,
        name = game.Name,
        description = game.Description,
        category = game.Category,
        developer = game.Developer,
        publisher = game.Publisher,
        genres = game.Genres,
        platforms = game.Platforms,
        tags = game.Tags,
        price = game.Price,
        releaseDate = game.ReleaseDate,
        rating = game.Rating
    };

    private static IReadOnlyCollection<GameSearchResult> ParseResults(string json)
    {
        using var document = JsonDocument.Parse(json);
        var results = new List<GameSearchResult>();
        foreach (var hit in document.RootElement.GetProperty("hits").GetProperty("hits").EnumerateArray())
        {
            var source = hit.GetProperty("_source");
            results.Add(new GameSearchResult(
                source.GetProperty("id").GetGuid(),
                source.GetProperty("name").GetString() ?? string.Empty,
                source.GetProperty("description").GetString() ?? string.Empty,
                source.GetProperty("category").GetString() ?? string.Empty,
                source.GetProperty("developer").GetString() ?? string.Empty,
                source.GetProperty("publisher").GetString() ?? string.Empty,
                ReadStrings(source, "genres"),
                ReadStrings(source, "platforms"),
                ReadStrings(source, "tags"),
                source.GetProperty("price").GetDecimal(),
                hit.TryGetProperty("_score", out var score) && score.ValueKind == JsonValueKind.Number ? score.GetDouble() : 0));
        }

        return results;
    }

    private static IReadOnlyCollection<string> ReadStrings(JsonElement source, string propertyName) =>
        source.TryGetProperty(propertyName, out var value)
            ? value.EnumerateArray().Select(item => item.GetString() ?? string.Empty).ToArray()
            : Array.Empty<string>();

    private static void EnsureSuccess(StringResponse response, string operation)
    {
        if (!response.Success)
            throw new InvalidOperationException($"OpenSearch failed to {operation}: HTTP {response.HttpStatusCode}. {response.Body}");
    }
}
