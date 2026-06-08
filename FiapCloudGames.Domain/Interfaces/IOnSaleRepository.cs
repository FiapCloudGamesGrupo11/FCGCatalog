using FiapCloudGames.Domain.Entity;

namespace FiapCloudGames.Domain.Interfaces
{
    public interface IOnSaleRepository
    {
        Task<OnSale?> GetByIdAsync (Guid id);
        Task<IEnumerable<OnSale>> GetAllAsync ();
        Task AddAsync (OnSale entity);
        Task UpdateAsync (OnSale entity);
    }
}
