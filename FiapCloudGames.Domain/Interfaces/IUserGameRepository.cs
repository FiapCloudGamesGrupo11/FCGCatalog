using FiapCloudGames.Domain.Entity;

namespace FiapCloudGames.Domain.Interfaces
{
    public interface IUserGameRepository
    {
        Task<UsersGames> Create(UsersGames userGame);
    }
}
