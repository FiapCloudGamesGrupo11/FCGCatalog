using FiapCloudGames.Application.DTOs.Game.Request;
using FiapCloudGames.Application.DTOs.Game.Response;
using FiapCloudGames.Application.Interfaces;
using FiapCloudGames.Application.Results;
using FiapCloudGames.Domain.Entity;
using FiapCloudGames.Domain.Interfaces;

namespace FiapCloudGames.Application.Services
{
    public class GameService : IGameService
    {
        private readonly IValidationBehavior<GameRequest> _validation;
        private readonly IGameRepository _gameRepository;

        public GameService(IValidationBehavior<GameRequest> validation, IGameRepository gameRepository)
        {
            _validation = validation;
            _gameRepository = gameRepository;

        }

        public async Task<GameCreatedResponse> CreateGame (GameRequest request)
        {
            //await _validation.ValidateAsync(request);

            var Game = new Game(request.Name, request.Price, request.Description, request.Category);
            var response = await _gameRepository.AddAsync(Game);
            var CreateGame = new GameCreatedResponse(response.Name, response.Price, response.Description, response.Category);

            return Result<GameCreatedResponse>.Success(CreateGame).Value;
        }
        public async Task<IEnumerable<GameCreatedResponse>> GetAllAsync ()
        {

            var games = await _gameRepository.GetAllAsync();

            foreach (var item in games)
            {

                if (item.OnSales.Count > 0)
                {

                    var onSale = item.OnSales.FirstOrDefault();
                    if (onSale != null)
                    {
                        item.Price = onSale.GetDiscountedPrice();
                    }
                }
            }
            return games.Select(g => new GameCreatedResponse(g.Name, g.Price, g.Description, g.Category));
        }

        public async Task<GameCreatedResponse> GetGameById (Guid id)
        {
            var response = await _gameRepository.GetGameByID(id);

            if (response.OnSales.Count > 0)
            {

                var onSale = response.OnSales.FirstOrDefault();
                if (onSale != null)
                {
                    response.Price = onSale.GetDiscountedPrice();
                }
            }


            var gameResponse = Parse(response);
            return Result<GameCreatedResponse>.Success(gameResponse).Value;
        }


        public async Task<GameCreatedResponse> UpdateGame(Guid id, GameRequest request)
        {
            var existingGame = await _gameRepository.GetGameByID(id);

            existingGame.Name = request.Name != null ? request.Name : existingGame.Name;
            existingGame.Price = request.Price != null ? request.Price : existingGame.Price;
            existingGame.Description = request.Description != null ? request.Description : existingGame.Description;
            existingGame.Category = request.Category != null ? request.Category : existingGame.Category;

            var result = await _gameRepository.UpdateGameAsync(existingGame);

            return Result<GameCreatedResponse>.Success(new GameCreatedResponse(existingGame.Name, existingGame.Price, existingGame.Description, existingGame.Category)).Value;
        }


        private static GameCreatedResponse Parse(Game game)
        {
            return new GameCreatedResponse(game.Name, game.Price, game.Description, game.Category);
        }
    }
}
