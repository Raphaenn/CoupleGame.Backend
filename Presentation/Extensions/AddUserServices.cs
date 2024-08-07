using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Domain.Services;
using Infrastructure.Repository.Database;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation.Extensions;
public static class UserServiceCollectionExtensions
{
    public static IServiceCollection AddUserService(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserAppService, UserAppService>();
        services.AddScoped<UserService>();
        
        return services;
    }
}