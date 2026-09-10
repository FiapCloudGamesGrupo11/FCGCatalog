using FiapCloudGames.Application.DTOs.Game.Request;
using FiapCloudGames.Application.DTOs.Game.Response;
using FiapCloudGames.Application.Interfaces;
using FiapCloudGames.Application.Services;
using FiapCloudGames.Domain.Entity;
using FiapCloudGames.Domain.Interfaces;
using Moq;

namespace FiapCloudGames.Testes.Services;

public class GameServiceTests
{
    private readonly Mock<IMongoGameRepository> _mongoGameRepositoryMock;
    private readonly Mock<IOnSaleRepository> _onSaleRepositoryMock;
    private readonly Mock<IValidationBehavior<GameRequest>> _validationBehavior;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly GameService _gameService;

    public GameServiceTests()
    {
        _mongoGameRepositoryMock = new Mock<IMongoGameRepository>();
        _onSaleRepositoryMock = new Mock<IOnSaleRepository>();
        _validationBehavior = new Mock<IValidationBehavior<GameRequest>>();
        _cacheServiceMock = new Mock<ICacheService>();
        _cacheServiceMock.Setup(c => c.GetAsync<IEnumerable<GameCreatedResponse>>(It.IsAny<string>()))
                         .ReturnsAsync((IEnumerable<GameCreatedResponse>?)null);
        _cacheServiceMock.Setup(c => c.GetAsync<GameResponseFull>(It.IsAny<string>()))
                         .ReturnsAsync((GameResponseFull?)null);

        _onSaleRepositoryMock.Setup(r => r.GetAllAsync())
                             .ReturnsAsync(new List<OnSale>());

        _gameService = new GameService(
            _validationBehavior.Object,
            _mongoGameRepositoryMock.Object,
            _onSaleRepositoryMock.Object,
            _cacheServiceMock.Object
        );
    }

    private static GameFullData CreateGame(string name, decimal price, string description, string category)
    {
        return GameFullData.TransferData(
            name,
            description,
            category,
            "Developer",
            "Publisher",
            new List<string>(),
            new List<string>(),
            price,
            DateTime.Now,
            "E",
            new List<string>());
    }

    [Fact]
    public async Task CreateGame_ShouldReturnGameCreatedResponse_WhenValidRequest()
    {
        // Arrange
        var request = new GameRequest
        {
            Name = "Super Mario",
            Price = 299.99m,
            Description = "Classic game",
            Category = "Platform"
        };

        _mongoGameRepositoryMock.Setup(r => r.AddAsync(It.IsAny<GameFullData>()))
                                .Returns(Task.CompletedTask);

        // Act
        var result = await _gameService.CreateGame(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Super Mario", result.Name);
        Assert.Equal(299.99m, result.Price);
        Assert.Equal("Classic game", result.Description);
        Assert.Equal("Platform", result.Category);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnListOfGames_WhenGamesExist()
    {
        // Arrange
        var games = new List<GameFullData>
        {
            CreateGame("Game 1", 100m, "Desc 1", "Action"),
            CreateGame("Game 2", 200m, "Desc 2", "RPG")
        };

        _mongoGameRepositoryMock.Setup(r => r.GetAllAsync())
                           .ReturnsAsync(games);

        // Act
        var result = await _gameService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal("Game 1", result.First().Name);
        Assert.Equal("Game 2", result.Last().Name);
    }

    [Fact]
    public async Task GetGameById_ShouldReturnGame_WhenGameExists()
    {
        // Arrange
        var expectedGame = CreateGame("Game XYZ", 150m, "Desc XYZ", "Strategy");

        _mongoGameRepositoryMock.Setup(r => r.GetByIdAsync(expectedGame.Id))
                           .ReturnsAsync(expectedGame);

        // Act
        var result = await _gameService.GetGameById(expectedGame.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Game XYZ", result.Name);
        Assert.Equal(150m, result.Price);
        Assert.Equal("Strategy", result.Category);
    }
}

