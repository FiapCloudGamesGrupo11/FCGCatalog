using FiapCloudGames.Application.DTOs.User.Request;
using FiapCloudGames.Application.DTOs.User.Response;
using FiapCloudGames.Application.Interfaces;
using FiapCloudGames.Application.Services;
using FiapCloudGames.Domain.Entity;
using FiapCloudGames.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Reqnroll;

namespace FiapCloudGames.Testes
{
    [Binding]
    public class CreateUserStepDefinitions
    {
        private UserRequestView _request;
        private UserCreatedResponseView _response;
        private UserService _service;

        private Mock<IValidationBehavior<UserRequestView>> _validationCreateMock;
        private Mock<IValidationBehavior<UserRequestUpdateView>> _validationUpdateMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IAuthHelpers> _authHelpersMock;

        public CreateUserStepDefinitions()
        {
            _validationCreateMock = new Mock<IValidationBehavior<UserRequestView>>();
            _validationUpdateMock = new Mock<IValidationBehavior<UserRequestUpdateView>>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _authHelpersMock = new Mock<IAuthHelpers>();

            // Setup padrão (não fazer nada na validação)
            _validationCreateMock
                .Setup(v => v.ValidateAsync(It.IsAny<UserRequestView>()))
                .Returns(Task.CompletedTask);

            // Mock do hash da senha
            _authHelpersMock
                .Setup(a => a.ComputeSha256Hash(It.IsAny<string>()))
                .Returns("hashed-password");

            // Mock do repository (simula persistência)
            _userRepositoryMock
                .Setup(r => r.Create(It.IsAny<User>()))
                .ReturnsAsync((User u) =>
                {
                    // simula que o banco gerou um Id
                    return new User(u.Name, u.LastName, u.Email, u.Password)
                    {
                        Id = Guid.NewGuid()
                    };
                });

            // Instancia o service com mocks
            _service = new UserService(
                _validationCreateMock.Object,
                _validationUpdateMock.Object,
                _userRepositoryMock.Object,
                _authHelpersMock.Object
            );
        }

        [Given("a valid user request")]
        public void GivenAValidUserRequest()
        {
            _request = new UserRequestView
            {
                Name = "Anderson",
                LastName = "Teste",
                Email = "teste@email.com",
                Password = "123456"
            };
        }

        [When("I create the user")]
        public async Task WhenICreateTheUser()
        {
            _response = await _service.CreateUser(_request);
        }

        [Then("the user should be created successfully")]
        public void ThenTheUserShouldBeCreatedSuccessfully()
        {
            _response.Should().NotBeNull();
        }

        [Then("the response should contain user data")]
        public void ThenTheResponseShouldContainUserData()
        {
            _response.Name.Should().Be("Anderson");
            _response.Email.Should().Be("teste@email.com");
            _userRepositoryMock.Verify(r => r.Create(It.IsAny<User>()), Times.Once);
            _authHelpersMock.Verify(a => a.ComputeSha256Hash("123456"), Times.Once);
        }

    }
}
