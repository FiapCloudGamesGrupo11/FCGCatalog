using FiapCloudGames.Application.Services;
using FiapCloudGames.Domain.Entity;
using FiapCloudGames.Domain.Interfaces;
using FiapCloudGames.Infrastructure.MessageBus;
using Moq;

namespace FiapCloudGames.Testes.Services;

public class UserGameServiceTests
{
    private readonly Mock<IUserGameRepository> _userGameRepositoryMock;
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly Mock<IMessagePublisher> _messagePublisherMock;
    private readonly RabbitMqSettings _rabbitMqSettings;
    private readonly UserGameService _userGameService;

    public UserGameServiceTests()
    {
        _userGameRepositoryMock = new Mock<IUserGameRepository>();
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _messagePublisherMock = new Mock<IMessagePublisher>();
        _rabbitMqSettings = new RabbitMqSettings { OrderQueueName = "order-placed" };

        _userGameService = new UserGameService(
            _userGameRepositoryMock.Object,
            _orderRepositoryMock.Object,
            _messagePublisherMock.Object,
            _rabbitMqSettings
        );
    }

    [Fact]
    public async Task AddGameToUser_ShouldCallRepositoryCreate_AndPublishMessage()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var gameId = Guid.NewGuid();
        var valuePay = 199.90m;

        var createdUserGame = new UsersGames(userId, gameId, valuePay);

        _userGameRepositoryMock
            .Setup(r => r.Create(It.IsAny<UsersGames>()))
            .ReturnsAsync(createdUserGame);

        // Act
        await _userGameService.AddGameToUser(userId, gameId, valuePay);

        // Assert - repository called once with expected values
        _userGameRepositoryMock.Verify(r => r.Create(It.Is<UsersGames>(ug =>
            ug.UserId == userId &&
            ug.GameId == gameId &&
            ug.ValuePay == valuePay
        )), Times.Once);

        // Assert - message published once to the configured queue with a non-empty payload
        _messagePublisherMock.Verify(p => p.PublishAsync(
            It.Is<string>(q => q == _rabbitMqSettings.OrderQueueName),
            It.Is<byte[]>(msg => msg != null && msg.Length > 0)
        ), Times.Once);
    }
}
