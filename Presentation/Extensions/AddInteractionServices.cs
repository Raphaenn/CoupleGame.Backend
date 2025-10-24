using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Repository.Database;

namespace Api.Extensions;

public static class AddInteractionServicesCollectionsExtensions
{
    public static IServiceCollection AddInteractionServices(this IServiceCollection service)
    {
        service.AddScoped<IInteractionAppService, InteractionAppService>();
        service.AddScoped<IInteractionsRepository, InteractionRepository>();

        return service;
    }
}