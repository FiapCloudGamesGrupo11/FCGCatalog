using FiapCloudGames.API.Controllers;
using FiapCloudGames.Domain.Interfaces;
using FiapCloudGames.Domain.Search;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FiapCloudGames.Testes.Controllers;

public sealed class SearchControllerTests
{
    private readonly Mock<IGameSearchService> _searchService = new();
    private readonly Mock<IMongoGameRepository> _gameRepository = new();

    [Fact]
    public async Task Search_ReturnsBadRequest_WhenQueryIsEmpty()
    {
        var controller = new SearchController(_searchService.Object, _gameRepository.Object);

        var result = await controller.Search(" ", 1, 20);

        Assert.IsType<BadRequestObjectResult>(result);
        _searchService.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Search_ReturnsResultsOrderedByOpenSearch()
    {
        var expected = new[]
        {
            new GameSearchResult(
                Guid.NewGuid(), "The Witcher 3", "RPG", "RPG", "CDPR", "CDPR",
                new[] { "RPG" }, new[] { "PC" }, new[] { "fantasia" }, 100m, 8.7)
        };
        _searchService
            .Setup(service => service.SearchAsync("witchr", 1, 20, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        var controller = new SearchController(_searchService.Object, _gameRepository.Object);

        var result = await controller.Search(" witchr ", 1, 20);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Same(expected, ok.Value);
    }
}
