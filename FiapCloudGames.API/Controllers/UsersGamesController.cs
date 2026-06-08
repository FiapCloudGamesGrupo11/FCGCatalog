using FiapCloudGames.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FiapCloudGames.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersGamesController : ControllerBase
    {
        private readonly IUserGameService _userGameService;

        public UsersGamesController (IUserGameService userGameService)
        {
            _userGameService = userGameService;
        }

        [HttpPost]
        [Route("[action]")]
        [SwaggerOperation(Summary = "Add new Game to User.")]
        public async Task<IActionResult> Post (Guid userId, Guid gameId, decimal valuePay)
        {
            await _userGameService.AddGameToUser(userId, gameId, valuePay);
            return Ok();
        }
    }
}
