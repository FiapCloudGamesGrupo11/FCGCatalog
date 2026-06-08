using FiapCloudGames.Application.DTOs.User.Request;
using FiapCloudGames.Application.DTOs.User.Response;
using FiapCloudGames.Application.Interfaces;
using FiapCloudGames.Application.Results;
using FiapCloudGames.Domain.Entity;
using FiapCloudGames.Domain.Enums;
using FiapCloudGames.Domain.Interfaces;

namespace FiapCloudGames.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IValidationBehavior<UserRequestView> _validationCreate;
        private readonly IValidationBehavior<UserRequestUpdateView> _validationUpdate;
        private readonly IUserRepository _userRepository;
        private readonly IAuthHelpers _authHelpers;

        public UserService(IValidationBehavior<UserRequestView> validationCreate, IValidationBehavior<UserRequestUpdateView> validationUpdate, IUserRepository userRepository, IAuthHelpers authHelpers)
        {
            _validationCreate = validationCreate;
            _validationUpdate = validationUpdate;
            _userRepository = userRepository;
            _authHelpers = authHelpers;
        }

        public async Task<UserCreatedResponseView> CreateUser(UserRequestView request)
        {
            await _validationCreate.ValidateAsync(request);

            var userExist = await _userRepository.GetByEmail(request.Email);
            if (userExist != null) return null;


            request.Password = _authHelpers.ComputeSha256Hash(request.Password);

            var User = new User(request.Name, request.LastName, request.Email, request.Password);

            var response = await _userRepository.Create(User);

            var UserCriado = new UserCreatedResponseView(response.Id, response.Name, response.LastName, response.Email);

            return Result<UserCreatedResponseView>.Success((UserCreatedResponseView)UserCriado).Value;
        }

        public async Task<UserCreatedResponseView> CreateAdmin(UserRequestView request)
        {
            await _validationCreate.ValidateAsync(request);

            request.Password = _authHelpers.ComputeSha256Hash(request.Password);

            var User = new User(request.Name, request.LastName, request.Email, request.Password, Role.Admin);

            var response = await _userRepository.Create(User);

            var UserCriado = new UserCreatedResponseView(response.Id, response.Name, response.LastName, response.Email);

            return Result<UserCreatedResponseView>.Success((UserCreatedResponseView)UserCriado).Value;
        }

        public async Task<LoginResponseView> Login(LoginRequestView request)
        {
            var passHash = _authHelpers.ComputeSha256Hash(request.Password);
            var userResult = await _userRepository.GetByEmailPass(request.Email, passHash);

            if (userResult == null) return null;

            var token = _authHelpers.GenerateJwtToken(userResult.Email, userResult.Role.ToString());
            var userView = new LoginResponseView(userResult.Email, token);

            return userView;
        }

        public async Task<UserCreatedResponseView> GetUserById(Guid id)
        {
            var response = await _userRepository.GetById(id);
            if (response == null) return null;

            var userResponse = new UserCreatedResponseView(response.Id, response.Name, response.LastName, response.Email);

            return Result<UserCreatedResponseView>.Success((UserCreatedResponseView)userResponse).Value;
        }

        public async Task<IList<UserCreatedResponseView>> GetAll()
        {
            var response = await _userRepository.GetAll();
            if (response == null) return null;
            var userResponse = response.Select(r => new UserCreatedResponseView(r.Id, r.Name, r.LastName, r.Email)).ToList();
            return Result<IList<UserCreatedResponseView>>.Success(userResponse).Value;
        }

        public async Task<UserCreatedResponseView> UpdateRole(Guid id, Role requestRole)
        {
            var user = await _userRepository.GetById(id);
            if (user == null) return null;

            switch (requestRole)
            {
                case Role.Common:
                    user.ChangeRoleToCommon();
                    break;
                case Role.Admin:
                    user.ChangeRoleToAdmin();
                    break;
                default:
                    user.ChangeRoleToCommon();
                    break;
            }

            var result = await _userRepository.Update(user);

            var userResponse = new UserCreatedResponseView(result.Id, result.Name, result.LastName, result.Email);

            return userResponse;
        }

        public async Task<UserCreatedResponseView> UpdateStatus(Guid id, Status requestStatus)
        {
            var user = await _userRepository.GetById(id);
            if (user == null) return null;

            switch (requestStatus)
            {
                case Status.Active:
                    user.ActiveUser();
                    break;
                case Status.Blocked:
                    user.BlockUser();
                    break;
                case Status.Banned:
                    user.BanUser();
                    break;
                default:
                    user.BlockUser();
                    break;
            }

            var result = await _userRepository.Update(user);

            var userResponse = new UserCreatedResponseView(result.Id, result.Name, result.LastName, result.Email);

            return userResponse;
        }

        public async Task<UserCreatedResponseView> UpdateUser(Guid id, UserRequestUpdateView userRequest)
        {
            await _validationUpdate.ValidateAsync(userRequest);

            var user = await _userRepository.GetById(id);
            if (user == null) return null;

            user.Name = userRequest.Name != null ? userRequest.Name : user.Name;
            user.LastName = userRequest.LastName != null ? userRequest.LastName : user.LastName;

            var result = await _userRepository.Update(user);
            return new UserCreatedResponseView(result.Id, result.Name, result.LastName, result.Email);
        }
    }
}