using FiapCloudGames.Application.Services;
using FiapCloudGames.Domain.Entity;
using FiapCloudGames.Domain.Interfaces;
using Moq;

namespace FiapCloudGames.Testes.Services;

public class UserGameServiceTests
{
    private readonly Mock<IUserGameRepository> _userGameRepositoryMock;
    private readonly UserGameService _userGameService;

    public UserGameServiceTests()
    {
        _userGameRepositoryMock = new Mock<IUserGameRepository>();

        _userGameService = new UserGameService(
            _userGameRepositoryMock.Object
        );
    }

    [Fact]
    public async Task AddGameToUser_ShouldCallRepositoryCreate_WithCorrectData()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var gameId = Guid.NewGuid();
        var valuePay = 199.90m;

        _userGameRepositoryMock.Setup(r => r.Create(It.IsAny<UsersGames>()))
                               .ReturnsAsync(new UsersGames(userId, gameId, valuePay));

        // Act
        await _userGameService.AddGameToUser(userId, gameId, valuePay);

        // Assert
        _userGameRepositoryMock.Verify(r => r.Create(It.Is<UsersGames>(ug => 
            ug.UserId == userId && 
            ug.GameId == gameId && 
            ug.ValuePay == valuePay
        )), Times.Once);
    }
}
