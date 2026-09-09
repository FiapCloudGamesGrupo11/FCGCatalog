using FiapCloudGames.Application.DTOs.OnSale.Request;
using FiapCloudGames.Application.DTOs.OnSale.Response;
using FiapCloudGames.Application.Interfaces;
using FiapCloudGames.Domain.Entity;
using FiapCloudGames.Domain.Enums;
using FiapCloudGames.Domain.Interfaces;

namespace FiapCloudGames.Application.Services
{
    public class OnSaleService : IOnSaleService
    {
        private readonly IOnSaleRepository _repository;
        private readonly IMongoGameRepository _repositoryGame;

        public OnSaleService (IOnSaleRepository repository, IMongoGameRepository repositoryGame)
        {
            _repository = repository;
            _repositoryGame = repositoryGame;
        }

        public async Task<IEnumerable<OnSaleResponse>> GetAllAsync ()
        {
            var sales = await _repository.GetAllAsync();
            var games = (await _repositoryGame.GetAllAsync()).ToDictionary(game => game.Id);
            var now = DateTime.Now;

            return sales
                .Where(sale => sale.StartDate <= now &&
                              sale.EndDate >= now &&
                              games.ContainsKey(sale.GameId))
                .Select(sale =>
                {
                    var game = games[sale.GameId];
                    return new OnSaleResponse
                    {
                        Id = sale.Id,
                        GameName = game.Name,
                        OriginalPrice = game.Price,
                        DiscountPercentage = sale.DiscountPercentage,
                        DiscountedPrice = GetDiscountedPrice(game.Price, sale)
                    };
                });
        }

        public async Task<OnSaleResponse?> GetByIdAsync (Guid id)
        {
            var sale = await _repository.GetByIdAsync(id);
            if (sale == null) return null;

            var game = await _repositoryGame.GetByIdAsync(sale.GameId);
            if (game is null) return null;

            return new OnSaleResponse
            {
                Id = sale.Id,
                GameName = game.Name,
                OriginalPrice = game.Price,
                DiscountPercentage = sale.DiscountPercentage,
                DiscountedPrice = GetDiscountedPrice(game.Price, sale)
            };
        }

        public async Task<OnSaleResponse> CreateAsync (OnSaleRequest request)
        {
            var sale = new OnSale
            {
                Id = Guid.NewGuid(),
                GameId = request.GameId,
                DiscountPercentage = request.DiscountPercentage,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Status = GetStatus(request.StartDate, request.EndDate)
            };

            var game = await _repositoryGame.GetByIdAsync(request.GameId);
            if (game is null) return null;

            await _repository.AddAsync(sale);

            return new OnSaleResponse
            {
                Id = sale.Id,
                GameName = game.Name,
                OriginalPrice = game.Price,
                DiscountPercentage = sale.DiscountPercentage,
                DiscountedPrice = GetDiscountedPrice(game.Price, sale)

            };
            
        }

        public async Task<OnSaleResponse> UpdateAsync (Guid id, OnSaleRequest request)
        {
            var sale = await _repository.GetByIdAsync(id);
            if (sale == null) throw new Exception("OnSale not found");

            var game = await _repositoryGame.GetByIdAsync(request.GameId);
            if (game is null) throw new KeyNotFoundException("Jogo não encontrado");

            sale.GameId = request.GameId;
            sale.DiscountPercentage = request.DiscountPercentage;
            sale.StartDate = request.StartDate;
            sale.EndDate = request.EndDate;
            sale.Status = GetStatus(request.StartDate, request.EndDate);

            await _repository.UpdateAsync(sale);

            return CreateResponse(sale, game);
        }

        private static OnSaleResponse CreateResponse(OnSale sale, GameFullData game)
        {
            return new OnSaleResponse
            {
                Id = sale.Id,
                GameName = game.Name,
                OriginalPrice = game.Price,
                DiscountPercentage = sale.DiscountPercentage,
                DiscountedPrice = GetDiscountedPrice(game.Price, sale)
            };
        }

        private static decimal GetDiscountedPrice(decimal originalPrice, OnSale sale)
        {
            return sale.Status == Status.Active &&
                   DateTime.Now >= sale.StartDate &&
                   DateTime.Now <= sale.EndDate
                ? originalPrice - (originalPrice * sale.DiscountPercentage / 100)
                : originalPrice;
        }

        private static Status GetStatus(DateTime startDate, DateTime endDate)
        {
            return DateTime.Now >= startDate && DateTime.Now <= endDate
                ? Status.Active
                : Status.Desactivated;
        }

    }
}
