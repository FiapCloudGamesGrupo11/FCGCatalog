using FiapCloudGames.Application.DTOs.OnSale.Request;
using FiapCloudGames.Application.DTOs.OnSale.Response;

namespace FiapCloudGames.Application.Interfaces
{
    public interface IOnSaleService
    {
        Task<IEnumerable<OnSaleResponse>> GetAllAsync ();
        Task<OnSaleResponse?> GetByIdAsync (Guid id);
        Task<OnSaleResponse> CreateAsync (OnSaleRequest request);
        Task<OnSaleResponse> UpdateAsync (Guid id, OnSaleRequest request);
    }
}
