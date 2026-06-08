using FiapCloudGames.Application.DTOs.User.Request;
using FiapCloudGames.Application.Interfaces;
using FiapCloudGames.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FiapCloudGames.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
   
        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Create a new user common.", Description = "Create a new user using the provided information. The email address must be unique. General access.")]
        public async Task<IActionResult> CreateUser([FromBody]UserRequestView request)
        {
            var result = await _userService.CreateUser(request);
            if (result == null) return BadRequest(new ProblemDetails
            {
                Title = "Email já cadastrado.",
                Status = StatusCodes.Status400BadRequest
            });

            return Ok(result);
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize(Roles ="Admin")]
        [SwaggerOperation(Summary = "Create a new user admin.", Description = "Create a new user using the provided information. The email address must be unique. Only admins have access.")]
        public async Task<IActionResult> CreateUserAdmin([FromBody] UserRequestView request)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if(role != "Admin") return Forbid();

            var result = await _userService.CreateAdmin(request);
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]")]
        [SwaggerOperation(Summary = "Search for a user by ID.")]

        public async Task<IActionResult> GetUserById([FromQuery] Guid requestId)
        {
            var result = await _userService.GetUserById(requestId);
            if(result == null) return NotFound(new ProblemDetails
            {
                Title = "Usuário não encontrado",
                Status = StatusCodes.Status404NotFound
            });
            return Ok(result);  
        }

        [HttpGet]
        [Route("[action]")]
        [SwaggerOperation(Summary = "Search all users.")]
        public async Task<IActionResult> GetAll()
        {
            //ToDo - Paginação
            var result = await _userService.GetAll();
            if (result == null) return NotFound(new ProblemDetails
            {
                Title = "Usuário não encontrado",
                Status = StatusCodes.Status404NotFound
            });
            return Ok(result);
        }

        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Regular user login or admin.")]
        public async Task<IActionResult> Login(LoginRequestView request)
        {
            var response = await _userService.Login(request);
            if (response == null) return NotFound(new ProblemDetails
            {
                Title = "Usuário não encontrado",
                Status = StatusCodes.Status404NotFound
            });
            return Ok(response);
        }

        [HttpPut]
        [Route("[action]")]
        [SwaggerOperation(Summary = "Update user data.")]
        public async Task<IActionResult> UpdateUser([FromQuery] Guid requestId, [FromBody] UserRequestUpdateView request)
        {
            var result = await _userService.UpdateUser(requestId, request);
            if (result == null) return NotFound(new ProblemDetails
            {
                Title = "Usuário não encontrado",
                Status = StatusCodes.Status404NotFound
            });
            return Ok(result);
        }

        [HttpPut]
        [Route("[action]")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Update user role.  Only Admin access")]
        public async Task<IActionResult> UpdateRole([FromQuery] Guid requestId, [FromBody] Role requestRole)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role != "Admin") return Forbid();

            var result = await _userService.UpdateRole(requestId, requestRole);

            if (result == null) return NotFound(new ProblemDetails
            {
                Title = "Usuário não encontrado",
                Status = StatusCodes.Status404NotFound
            });
            return Ok(result);
        }

        [HttpPut]
        [Route("[action]")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Update user status.  Only Admin access")]
        public async Task<IActionResult> UpdateStatus([FromQuery] Guid requestId, [FromBody] Status requestStatus)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role != "Admin") return Forbid();

            var result = await _userService.UpdateStatus(requestId, requestStatus);

            if (result == null) return NotFound(new ProblemDetails
            {
                Title = "Usuário não encontrado",
                Status = StatusCodes.Status404NotFound
            });
            return Ok(result);
        }
    }
}
