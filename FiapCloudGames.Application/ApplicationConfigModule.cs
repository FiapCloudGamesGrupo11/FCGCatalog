using FiapCloudGames.Application.Behaviors;
using FiapCloudGames.Application.Interfaces;
using FiapCloudGames.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FiapCloudGames.Application
{
    public static class ApplicationConfigModule
    {
        public static IServiceCollection AddConfigServices(this IServiceCollection services)
        {
            services.AddScoped<IUserGameService, UserGameService>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<IOnSaleService, OnSaleService>();

            services.AddScoped(typeof(IValidationBehavior<>), typeof(ValidationBehavior<>));

            return services;
        }
    }
}
