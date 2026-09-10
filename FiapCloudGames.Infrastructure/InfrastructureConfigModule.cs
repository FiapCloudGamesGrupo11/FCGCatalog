using FiapCloudGames.Domain.Interfaces;
using FiapCloudGames.Infrastructure.Authorization;
using FiapCloudGames.Infrastructure.MessageBus;
using FiapCloudGames.Infrastructure.Persistence;
using FiapCloudGames.Infrastructure.Persistence.Mongo;
using FiapCloudGames.Infrastructure.Persistence.Mongo.Migrations;
using FiapCloudGames.Infrastructure.Persistence.Redis;
using FiapCloudGames.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FiapCloudGames.Infrastructure
{
    public static class InfrastructureConfigModule
    {
        public static IServiceCollection AddConfigInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddPersistence(configuration);
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            services.AddScoped<IUserGameRepository, UserGameRepository>();
            services.AddScoped<IAuthHelpers, AuthHelpers>();
            services.AddScoped<IOnSaleRepository, MongoOnSaleRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IMongoGameRepository, MongoGameRepository>();

            services.AddScoped<IMongoMigration, PopulateGamesAndOnSalesMigration>();
            services.AddScoped<MongoMigrationRunner>();

            services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();
            services.AddScoped<IMessagePublisher, RabbitMqPublisher>();
            services.AddSingleton<IRabbitMqConsumer, RabbitMqConsumer>();
            services.AddHostedService<RabbitMqWorker>();

            services.AddScoped<ICacheService, RedisCacheService>();

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

            var redisSettings =
                configuration
                .GetSection("Redis")
                .Get<RedisSettings>() ?? new RedisSettings();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisSettings.ConnectionString;
                options.InstanceName = redisSettings.InstanceName;
            });

            return services;
        }

        // Deve ser chamado após o host ser construído (builder.Build()), nunca durante o registro de serviços,
        // para evitar criar um IServiceProvider "fantasma" separado do container real da aplicação.
        public static async Task MigrateAndSeedInfrastructureAsync(this IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>()
                .CreateLogger("InfrastructureConfigModule");

            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await db.Database.MigrateAsync();
            logger.LogInformation("SQL Server migrations applied successfully.");

            var mongoMigrationRunner = scope.ServiceProvider.GetRequiredService<MongoMigrationRunner>();
            await mongoMigrationRunner.MigrateAsync();
            logger.LogInformation("MongoDB migrations applied successfully.");
        }
    }
}
