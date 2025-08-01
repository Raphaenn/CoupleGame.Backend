using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Repository.Database;

namespace Api.Extensions;

public static class InviteServiceCollectionExtensions
{
    public static IServiceCollection AddInviteServices(this IServiceCollection services)
    {
        services.AddScoped<IInviteAppService, InviteAppService>();
        services.AddScoped<IInviteRepository, InviteRepository>();

        return services;
    }
}