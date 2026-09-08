using FiapCloudGames.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGames.Domain.Interfaces
{
    public interface IApplicationDbContext
    {
        Task<int> SaveChangesAsync (CancellationToken cancellationToken = default);
        DbSet<User> Users { get; }
        DbSet<UsersGames> UsersGames { get; }
        DbSet<Order> Orders { get; set; }

    }
}
