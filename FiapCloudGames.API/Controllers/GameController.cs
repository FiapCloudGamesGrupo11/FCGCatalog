using FiapCloudGames.Application.DTOs.Game.Request;
using FiapCloudGames.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FiapCloudGames.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        public GameController (IGameService gameService)
        {
            _gameService = gameService;
        }


        [HttpPost]
        [Route("[action]")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Create a new game.")]
        public async Task<IActionResult> CreateGame ([FromBody] GameRequest request)
        {
            var result = await _gameService.CreateGame(request);
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]")]
        [SwaggerOperation(Summary = "Search for game by ID.")]
        public async Task<IActionResult> GetGameByID ([FromQuery] Guid requestId)
        {
            var result = await _gameService.GetGameById(requestId);
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]")]
        [SwaggerOperation(Summary = "Get all games.")]
        public async Task<IActionResult> GetAllAsync ()
        {
            var result = await _gameService.GetAllAsync();
            return Ok(result);
        }

        [HttpPut]
        [Route("[action]")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Update a game.")]
        public async Task<IActionResult> UpdateGame ([FromQuery] Guid requestId, [FromBody] GameRequest request)
        {
            var result = await _gameService.UpdateGame(requestId, request);
            return Ok(result);
        }
    }
}
