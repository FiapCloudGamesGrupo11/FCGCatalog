using FiapCloudGames.Application.Behaviors;
using FiapCloudGames.Application.Interfaces;
using FiapCloudGames.Application.Services;
using FiapCloudGames.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace FiapCloudGames.Application
{
    public static class ApplicationConfigModule
    {
        public static IServiceCollection AddConfigServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserGameService, UserGameService>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<IOnSaleService, OnSaleService>();

            services.AddScoped(typeof(IValidationBehavior<>), typeof(ValidationBehavior<>));
            services.AddValidatorsFromAssembly(typeof(UserValidator).Assembly); 

            return services;
        }
    }
}
