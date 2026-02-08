using Application.Interfaces;
using Application.Services;
using Domain.Interfaces.IPce;
using Infrastructure.Repository.Database;

namespace Api.Extensions;

public static class PceServiceCollectionExtensions
{
    public static IServiceCollection AddPceServices(this IServiceCollection services)
    {
        services.AddScoped<IPceRepository, PceRepository>();
        services.AddScoped<IPceAnswersRepository, PceAnswersRepository>();
        services.AddScoped<IPceAppServices, PceAppService>();

        return services;
    } 
}