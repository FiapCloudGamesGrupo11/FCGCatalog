using FiapCloudGames.Application.DTOs.Game.Request;
using FiapCloudGames.Application.Interfaces;
using FiapCloudGames.Application.Services;
using FiapCloudGames.Domain.Entity;
using FiapCloudGames.Domain.Interfaces;
using Moq;

namespace FiapCloudGames.Testes.Services;

public class GameServiceTests
{
    private readonly Mock<IGameRepository> _gameRepositoryMock;
    private readonly Mock<IValidationBehavior<GameRequest>> _validationBehavior;
    private readonly GameService _gameService;

    public GameServiceTests()
    {
        _gameRepositoryMock = new Mock<IGameRepository>();
        _validationBehavior = new Mock<IValidationBehavior<GameRequest>>();

        _gameService = new GameService(
            _validationBehavior.Object,
            _gameRepositoryMock.Object
        );
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

        // ValidateAsync passará direto pois usamos uma lista vazia de validadores

        var expectedGame = new Game("Super Mario", 299.99m, "Classic game", "Platform");

        _gameRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Game>()))
                           .ReturnsAsync(expectedGame);

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
        var games = new List<Game>
        {
            new Game("Game 1", 100m, "Desc 1", "Action"),
            new Game("Game 2", 200m, "Desc 2", "RPG")
        };

        _gameRepositoryMock.Setup(r => r.GetAllAsync())
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
        var gameId = Guid.NewGuid();
        var expectedGame = new Game("Game XYZ", 150m, "Desc XYZ", "Strategy");

        _gameRepositoryMock.Setup(r => r.GetGameByID(gameId))
                           .ReturnsAsync(expectedGame);

        // Act
        var result = await _gameService.GetGameById(gameId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Game XYZ", result.Name);
        Assert.Equal(150m, result.Price);
        Assert.Equal("Strategy", result.Category);
    }
}
