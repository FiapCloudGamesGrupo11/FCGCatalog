using FiapCloudGames.Application.DTOs.UserGame;
using FiapCloudGames.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace FiapCloudGames.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersGamesController : ControllerBase
    {
        private readonly IUserGameService _userGameService;
        private readonly ILogger<UsersGamesController> _logger;

        public UsersGamesController(IUserGameService userGameService, ILogger<UsersGamesController> logger)
        {
            _userGameService = userGameService;
            _logger = logger;
        }

        [HttpPost]
        [Route("[action]")]
        [SwaggerOperation(Summary = "Add new Game to User.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] PurchaseGameRequest request)
        {
            if (request is null)
            {
                _logger.LogWarning("PurchaseGameRequest was null.");
                return BadRequest("Request body is required.");
            }

            if (request.UserId == Guid.Empty || request.GameId == Guid.Empty)
            {
                _logger.LogWarning("Invalid GUID in request. UserId: {UserId}, GameId: {GameId}", request.UserId, request.GameId);
                return BadRequest("UserId and GameId must be valid non-empty GUIDs.");
            }

            if (request.ValuePay < 0)
            {
                _logger.LogWarning("Invalid ValuePay: {ValuePay} for UserId: {UserId}, GameId: {GameId}", request.ValuePay, request.UserId, request.GameId);
                return BadRequest("ValuePay must be non-negative.");
            }

            await _userGameService.AddGameToUser(request.UserId, request.GameId, request.ValuePay);
            return NoContent();
        }
    }
}
