using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Domain.Services;
using Infrastructure.Repository.Database;

namespace Api.Extensions;

public static class AddUserPreferencesServiceCollectionExtensions
{
    public static IServiceCollection AddUserPrefService(this IServiceCollection services)
    {
        services.AddScoped<IUserPreferencesRepository, UserPreferencesRepository>();
        services.AddScoped<IUserPreferencesAppService, UserPreferencesAppService>();
        services.AddScoped<UserPreferenceService>();

        return services;
    }
}