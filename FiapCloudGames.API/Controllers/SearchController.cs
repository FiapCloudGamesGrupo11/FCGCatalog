using FiapCloudGames.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FiapCloudGames.API.Controllers;

[ApiController]
[Route("search")]
[Authorize]
public sealed class SearchController : ControllerBase
{
    private readonly IGameSearchService _searchService;
    private readonly IMongoGameRepository _gameRepository;

    public SearchController(IGameSearchService searchService, IMongoGameRepository gameRepository)
    {
        _searchService = searchService;
        _gameRepository = gameRepository;
    }

    [HttpGet]
    [SwaggerOperation(Summary = "Search games using relevance and typo tolerance.")]
    public async Task<IActionResult> Search(
        [FromQuery] string q,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(q))
            return BadRequest(new { message = "The q parameter is required." });
        if (page < 1 || pageSize is < 1 or > 100)
            return BadRequest(new { message = "page must be >= 1 and pageSize must be between 1 and 100." });

        var results = await _searchService.SearchAsync(q.Trim(), page, pageSize, cancellationToken);
        return Ok(results);
    }

    [HttpPost("reindex")]
    [Authorize(Roles = "Admin")]
    [SwaggerOperation(Summary = "Rebuild the OpenSearch game index from MongoDB.")]
    public async Task<IActionResult> Reindex(CancellationToken cancellationToken)
    {
        var games = await _gameRepository.GetAllAsync();
        var indexed = await _searchService.ReindexAsync(games, cancellationToken);
        return Ok(new { indexed });
    }
}
