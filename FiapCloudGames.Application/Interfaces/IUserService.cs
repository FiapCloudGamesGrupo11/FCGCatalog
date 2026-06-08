using FiapCloudGames.Application.DTOs.User.Request;
using FiapCloudGames.Application.DTOs.User.Response;
using FiapCloudGames.Domain.Enums;

namespace FiapCloudGames.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserCreatedResponseView> CreateUser(UserRequestView userRequest);
        Task<UserCreatedResponseView> GetUserById(Guid id);
        Task<IList<UserCreatedResponseView>> GetAll();
        Task<LoginResponseView> Login(LoginRequestView request);
        Task<UserCreatedResponseView> CreateAdmin(UserRequestView request);
        Task<UserCreatedResponseView> UpdateRole(Guid id, Role requestRole);
        Task<UserCreatedResponseView> UpdateStatus(Guid id, Status requestStatus);
        Task<UserCreatedResponseView> UpdateUser(Guid id, UserRequestUpdateView userRequest);
    }
}
