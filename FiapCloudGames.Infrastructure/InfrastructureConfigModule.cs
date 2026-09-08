using FiapCloudGames.Domain.Interfaces;
using FiapCloudGames.Infrastructure.Authorization;
using FiapCloudGames.Infrastructure.MessageBus;
using FiapCloudGames.Infrastructure.Persistence;
using FiapCloudGames.Infrastructure.Persistence.Mongo;
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
            services.AddScoped<IAuthHelpers, AuthHelpers>();
            services.AddScoped<IOnSaleRepository, MongoOnSaleRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IMongoGameRepository, MongoGameRepository>();

            services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();
            services.AddScoped<IMessagePublisher, RabbitMqPublisher>();
            services.AddSingleton<IRabbitMqConsumer, RabbitMqConsumer>();
            services.AddHostedService<RabbitMqWorker>();

            return services;
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            // Configurações de persistência, como DbContext, etc.
            var connectionString = configuration.GetConnectionString("ConnectionString");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            var rabbitSettings =
                configuration
                .GetSection("RabbitMq")
                .Get<RabbitMqSettings>();

            services.AddSingleton(rabbitSettings);

            services.Configure<MongoSettings>(
                configuration.GetSection("MongoSettings"));

            services.AddSingleton<MongoContext>();

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
