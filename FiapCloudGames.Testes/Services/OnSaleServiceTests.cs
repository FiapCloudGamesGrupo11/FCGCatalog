using FiapCloudGames.Application.DTOs.OnSale.Request;
using FiapCloudGames.Application.Services;
using FiapCloudGames.Domain.Entity;
using FiapCloudGames.Domain.Interfaces;
using Moq;
using Xunit;

namespace FiapCloudGames.Testes.Services;

public class OnSaleServiceTests
{
    private readonly Mock<IOnSaleRepository> _onSaleRepositoryMock;
    private readonly Mock<IGameRepository> _gameRepositoryMock;
    private readonly OnSaleService _onSaleService;

    public OnSaleServiceTests()
    {
        _onSaleRepositoryMock = new Mock<IOnSaleRepository>();
        _gameRepositoryMock = new Mock<IGameRepository>();

        _onSaleService = new OnSaleService(
            _onSaleRepositoryMock.Object,
            _gameRepositoryMock.Object
        );
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnListOfOnSale_WhenSalesExist()
    {
        // Arrange
        var game = new Game("Super Game", 100m, "Description", "Category");
        var sales = new List<OnSale>
        {
            new OnSale { Id = Guid.NewGuid(), GameId = game.Id, Game = game, DiscountPercentage = 20, StartDate = DateTime.Now.AddDays(-1), EndDate = DateTime.Now.AddDays(1), Status = Domain.Enums.Status.Active }
        };

        _onSaleRepositoryMock.Setup(r => r.GetAllAsync())
                             .ReturnsAsync(sales);

        // Act
        var result = await _onSaleService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        var first = result.First();
        Assert.Equal("Super Game", first.GameName);
        Assert.Equal(100m, first.OriginalPrice);
        Assert.Equal(80m, first.DiscountedPrice); // 20% discount
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnOnSaleResponse_WhenExists()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var game = new Game("Super Game", 200m, "Description", "Category");
        var expectedSale = new OnSale { Id = saleId, GameId = game.Id, Game = game, DiscountPercentage = 50, StartDate = DateTime.Now.AddDays(-1), EndDate = DateTime.Now.AddDays(1), Status = Domain.Enums.Status.Active };

        _onSaleRepositoryMock.Setup(r => r.GetByIdAsync(saleId))
                             .ReturnsAsync(expectedSale);

        // Act
        var result = await _onSaleService.GetByIdAsync(saleId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(saleId, result.Id);
        Assert.Equal("Super Game", result.GameName);
        Assert.Equal(100m, result.DiscountedPrice); // 50% discount
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenDoesNotExist()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        _onSaleRepositoryMock.Setup(r => r.GetByIdAsync(saleId))
                             .ReturnsAsync((OnSale?)null);

        // Act
        var result = await _onSaleService.GetByIdAsync(saleId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnOnSaleResponse_WhenValid()
    {
        // Arrange
        var request = new OnSaleRequest
        {
            GameId = Guid.NewGuid(),
            DiscountPercentage = 10,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(5)
        };

        var game = new Game("Super Game", 100m, "Description", "Category") { Id = request.GameId };

        _gameRepositoryMock.Setup(r => r.GetGameByID(request.GameId))
                           .ReturnsAsync(game);

        _onSaleRepositoryMock.Setup(r => r.AddAsync(It.IsAny<OnSale>()))
                             .Returns(Task.CompletedTask);

        // Act
        var result = await _onSaleService.CreateAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(10, result.DiscountPercentage);
        Assert.Equal(90m, result.DiscountedPrice); // 10% de 100m = 90m
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnNull_WhenGameNotFound()
    {
        // Arrange
        var request = new OnSaleRequest { GameId = Guid.NewGuid() };
        
        _gameRepositoryMock.Setup(r => r.GetGameByID(request.GameId))
                           .ReturnsAsync((Game?)null);

        // Act
        var result = await _onSaleService.CreateAsync(request);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateAndReturnResponse_WhenExists()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var game = new Game("Super Game", 100m, "Description", "Category");
        var existingSale = new OnSale { Id = saleId, GameId = game.Id, Game = game, DiscountPercentage = 10 };

        var request = new OnSaleRequest
        {
            GameId = game.Id,
            DiscountPercentage = 30,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(5)
        };

        _onSaleRepositoryMock.Setup(r => r.GetByIdAsync(saleId))
                             .ReturnsAsync(existingSale);

        _onSaleRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<OnSale>()))
                             .Returns(Task.CompletedTask);

        // Act
        var result = await _onSaleService.UpdateAsync(saleId, request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(30, result.DiscountPercentage);
        Assert.Equal(70m, result.DiscountedPrice);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenNotFound()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var request = new OnSaleRequest();

        _onSaleRepositoryMock.Setup(r => r.GetByIdAsync(saleId))
                             .ReturnsAsync((OnSale?)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _onSaleService.UpdateAsync(saleId, request));
    }
}
