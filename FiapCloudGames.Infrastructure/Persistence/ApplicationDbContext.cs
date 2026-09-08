using FiapCloudGames.Domain.Entity;
using FiapCloudGames.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FiapCloudGames.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<UsersGames> UsersGames { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
