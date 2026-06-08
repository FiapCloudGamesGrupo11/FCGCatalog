using FiapCloudGames.Application.DTOs.OnSale.Request;
using FiapCloudGames.Application.DTOs.OnSale.Response;
using FiapCloudGames.Application.Interfaces;
using FiapCloudGames.Domain.Entity;
using FiapCloudGames.Domain.Interfaces;
using System.Data;

namespace FiapCloudGames.Application.Services
{
    public class OnSaleService : IOnSaleService
    {
        private readonly IOnSaleRepository _repository;

        private readonly IGameRepository _repositoryGame;

        public OnSaleService (IOnSaleRepository repository, IGameRepository repositoryGame)
        {
            _repository = repository;
            _repositoryGame = repositoryGame;
        }

        public async Task<IEnumerable<OnSaleResponse>> GetAllAsync ()
        {
            var sales = await _repository.GetAllAsync();
            return sales.Select(s => new OnSaleResponse
            {
                Id = s.Id,
                GameName = s.Game?.Name ?? "",
                OriginalPrice = s.Game?.Price ?? 0,
                DiscountPercentage = s.DiscountPercentage,
                DiscountedPrice = s.GetDiscountedPrice()
            });
        }

        public async Task<OnSaleResponse?> GetByIdAsync (Guid id)
        {
            var sale = await _repository.GetByIdAsync(id);
            if (sale == null) return null;

            return new OnSaleResponse
            {
                Id = sale.Id,
                GameName = sale.Game?.Name ?? "",
                OriginalPrice = sale.Game?.Price ?? 0,
                DiscountPercentage = sale.DiscountPercentage,
                DiscountedPrice = sale.GetDiscountedPrice()
            };
        }

        public async Task<OnSaleResponse> CreateAsync (OnSaleRequest request)
        {
            var game = await _repositoryGame.GetGameByID(request.GameId);
            if (game == null) return null;


            var sale = new OnSale
            {
                Id = Guid.NewGuid(),
                GameId = game.Id,
                DiscountPercentage = request.DiscountPercentage,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };

            await _repository.AddAsync(sale);
            

            return new OnSaleResponse
            {
                Id = sale.Id,
                GameName = game.Name,
                OriginalPrice = game.Price,
                DiscountPercentage = sale.DiscountPercentage,
                DiscountedPrice = game.Price - (game.Price * sale.DiscountPercentage / 100)

            };
            
        }

        public async Task<OnSaleResponse> UpdateAsync (Guid id, OnSaleRequest request)
        {
            var sale = await _repository.GetByIdAsync(id);
            if (sale == null) throw new Exception("OnSale not found");

            sale.GameId = request.GameId;
            sale.DiscountPercentage = request.DiscountPercentage;
            sale.StartDate = request.StartDate;
            sale.EndDate = request.EndDate;

            await _repository.UpdateAsync(sale);

            return new OnSaleResponse
            {
                Id = sale.Id,
                GameName = sale.Game?.Name ?? "",
                OriginalPrice = sale.Game?.Price ?? 0,
                DiscountPercentage = sale.DiscountPercentage,
                DiscountedPrice = sale.GetDiscountedPrice()
            };
        }

    }
}
