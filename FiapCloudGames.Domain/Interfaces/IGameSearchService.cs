using FiapCloudGames.Domain.Entity;
using FiapCloudGames.Domain.Search;

namespace FiapCloudGames.Domain.Interfaces;

public interface IGameSearchService
{
    Task<IReadOnlyCollection<GameSearchResult>> SearchAsync(string query, int page, int pageSize, CancellationToken cancellationToken = default);
    Task IndexAsync(GameFullData game, CancellationToken cancellationToken = default);
    Task<int> ReindexAsync(IEnumerable<GameFullData> games, CancellationToken cancellationToken = default);
}
