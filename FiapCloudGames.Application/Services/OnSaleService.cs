using FiapCloudGames.Application.DTOs.OnSale.Request;
using FiapCloudGames.Application.DTOs.OnSale.Response;
using FiapCloudGames.Application.Interfaces;
using FiapCloudGames.Domain.Entity;
using FiapCloudGames.Domain.Enums;
using FiapCloudGames.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FiapCloudGames.Application.Services
{
    public class OnSaleService : IOnSaleService
    {
        private readonly IOnSaleRepository _repository;
        private readonly IMongoGameRepository _repositoryGame;
        private readonly IGameRepository? _legacyRepositoryGame;

        [ActivatorUtilitiesConstructor]
        public OnSaleService (IOnSaleRepository repository, IMongoGameRepository repositoryGame)
        {
            _repository = repository;
            _repositoryGame = repositoryGame;
        }

        public OnSaleService(IOnSaleRepository repository, IGameRepository repositoryGame)
        {
            _repository = repository;
            _repositoryGame = null!;
            _legacyRepositoryGame = repositoryGame;
        }

        public async Task<IEnumerable<OnSaleResponse>> GetAllAsync ()
        {
            var sales = await _repository.GetAllAsync();
            if (_legacyRepositoryGame is not null)
            {
                return sales.Select(CreateLegacyResponse);
            }

            var games = (await _repositoryGame.GetAllAsync()).ToDictionary(game => game.Id);

            return sales
                .Where(sale => games.ContainsKey(sale.GameId))
                .Select(sale => CreateResponse(sale, games[sale.GameId]));
        }

        public async Task<OnSaleResponse?> GetByIdAsync (Guid id)
        {
            var sale = await _repository.GetByIdAsync(id);
            if (sale == null) return null;

            if (_legacyRepositoryGame is not null)
            {
                return CreateLegacyResponse(sale);
            }

            var game = await _repositoryGame.GetByIdAsync(sale.GameId);
            return game is null ? null : CreateResponse(sale, game);
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

            if (_legacyRepositoryGame is not null)
            {
                var legacyGame = await _legacyRepositoryGame.GetGameByID(request.GameId);
                if (legacyGame is null) return null;

                await _repository.AddAsync(sale);

                return new OnSaleResponse
                {
                    Id = sale.Id,
                    GameName = legacyGame.Name,
                    OriginalPrice = legacyGame.Price,
                    DiscountPercentage = sale.DiscountPercentage,
                    DiscountedPrice = GetDiscountedPrice(legacyGame.Price, sale)
                };
            }

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

            if (_legacyRepositoryGame is not null)
            {
                sale.GameId = request.GameId;
                sale.DiscountPercentage = request.DiscountPercentage;
                sale.StartDate = request.StartDate;
                sale.EndDate = request.EndDate;
                await _repository.UpdateAsync(sale);
                return CreateLegacyResponse(sale);
            }

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

        private static OnSaleResponse CreateLegacyResponse(OnSale sale)
        {
            return new OnSaleResponse
            {
                Id = sale.Id,
                GameName = sale.Game?.Name ?? "",
                OriginalPrice = sale.Game?.Price ?? 0,
                DiscountPercentage = sale.DiscountPercentage,
                DiscountedPrice = sale.Game is null
                    ? 0
                    : GetDiscountedPrice(sale.Game.Price, sale)
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
