using FiapCloudGames.Application.DTOs.Game.Request;
using FiapCloudGames.Application.DTOs.Game.Response;
using FiapCloudGames.Application.Interfaces;
using FiapCloudGames.Application.Results;
using FiapCloudGames.Domain.Entity;
using FiapCloudGames.Domain.Enums;
using FiapCloudGames.Domain.Interfaces;

namespace FiapCloudGames.Application.Services
{
    public class GameService : IGameService
    {
        private readonly IValidationBehavior<GameRequest> _validation;
        private readonly IMongoGameRepository _mongoGameRepository;
        private readonly IOnSaleRepository _onSaleRepository;

        public GameService(
            IValidationBehavior<GameRequest> validation,
            IMongoGameRepository mongoGameRepository,
            IOnSaleRepository onSaleRepository)
        {
            _validation = validation;
            _mongoGameRepository = mongoGameRepository;
            _onSaleRepository = onSaleRepository;
        }

        public async Task<GameCreatedResponse> CreateGame (GameRequest request)
        {
            //await _validation.ValidateAsync(request);

            var game = GameFullData.TransferData(
                request.Name,
                request.Description,
                request.Category,
                request.Developer,
                request.Publisher,
                request.Genres,
                request.Platforms,
                request.Price,
                request.ReleaseDate,
                request.Rating,
                request.Tags);

            await _mongoGameRepository.AddAsync(game);

            var CreateGame = new GameCreatedResponse(game.Id, game.Name, game.Price, game.Description, game.Category);

            return Result<GameCreatedResponse>.Success(CreateGame).Value;
        }
        public async Task<IEnumerable<GameCreatedResponse>> GetAllAsync ()
        {
            var games = await _mongoGameRepository.GetAllAsync();
            var gameList = games.ToList();
            var sales = await _onSaleRepository.GetAllAsync();
            var responses = GameCreatedResponse.FromGameFullDataList(gameList);

            foreach (var response in responses)
            {
                var game = gameList.First(game => game.Id == response.Id);
                response.Price = GetPriceWithDiscount(game, sales);
            }

            return responses;
        }

        public async Task<GameResponseFull> GetGameById (Guid id)
        {
            var response = await _mongoGameRepository.GetByIdAsync(id);

            if (response is null)
            {
                throw new KeyNotFoundException($"Jogo com o ID {id} não foi encontrado.");
            }

            var gameResponse = GameResponseFull.FromGameFullData(response);
            var sales = await _onSaleRepository.GetAllAsync();
            gameResponse.Price = GetPriceWithDiscount(response, sales);
            return Result<GameResponseFull>.Success(gameResponse).Value;
        }


        private static decimal GetPriceWithDiscount(GameFullData game, IEnumerable<OnSale> sales)
        {
            var now = DateTime.Now;
            var sale = sales.FirstOrDefault(onSale =>
                onSale.GameId == game.Id &&
                onSale.Status == Status.Active &&
                now >= onSale.StartDate &&
                now <= onSale.EndDate);

            return sale is null
                ? game.Price
                : game.Price - (game.Price * sale.DiscountPercentage / 100);
        }

        public async Task<GameCreatedResponse> UpdateGame(Guid id, GameRequest request)
        {
            var existingGame = await _mongoGameRepository.GetByIdAsync(id);

            if (existingGame is null)
            {
                throw new KeyNotFoundException($"Jogo com o ID {id} não foi encontrado.");
            }

            var updatedGame = GameFullData.TransferData(
                request.Name,
                request.Description,
                request.Category,
                request.Developer,
                request.Publisher,
                request.Genres,
                request.Platforms,
                request.Price,
                request.ReleaseDate,
                request.Rating,
                request.Tags);
            updatedGame.Id = existingGame.Id;

            var result = await _mongoGameRepository.UpdateAsync(updatedGame);

            if (result is null)
            {
                throw new KeyNotFoundException($"Jogo com o ID {id} não foi encontrado.");
            }

            return Result<GameCreatedResponse>.Success(new GameCreatedResponse(
                result.Id,
                result.Name,
                result.Price,
                result.Description,
                result.Category)).Value;
        }

    }
}
