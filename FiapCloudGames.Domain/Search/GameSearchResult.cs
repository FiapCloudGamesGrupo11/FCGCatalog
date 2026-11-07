namespace FiapCloudGames.Domain.Search;

public sealed record GameSearchResult(
    Guid Id,
    string Name,
    string Description,
    string Category,
    string Developer,
    string Publisher,
    IReadOnlyCollection<string> Genres,
    IReadOnlyCollection<string> Platforms,
    IReadOnlyCollection<string> Tags,
    decimal Price,
    double Score);
