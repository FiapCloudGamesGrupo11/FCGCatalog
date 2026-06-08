using FiapCloudGames.Domain.Interfaces;
using FiapCloudGames.Infrastructure.Authorization;
using FiapCloudGames.Infrastructure.Persistence;
using FiapCloudGames.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FiapCloudGames.Infrastructure
{
    public static class InfrastructureConfigModule
    {
        public static IServiceCollection AddConfigInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPersistence(configuration);
            services.AddMigrations();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            services.AddScoped<IUserGameRepository, UserGameRepository>();
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IAuthHelpers, AuthHelpers>();
            services.AddScoped<IOnSaleRepository, OnSaleRepository>();

            return services;
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            // Configurações de persistência, como DbContext, etc.
            var connectionString = configuration.GetConnectionString("ConnectionString");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            return services;
        }

        private static IServiceCollection AddMigrations(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();

            using (var scope = provider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.Migrate();
            }
            return services;
        }
    }
}
