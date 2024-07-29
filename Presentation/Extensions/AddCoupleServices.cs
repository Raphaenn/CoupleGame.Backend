using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Domain.Services;
using Infrastructure.Repository.Database;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Extensions;
public static class CoupleServiceCollectionExtensions
{
    public static IServiceCollection AddCoupleServices(this IServiceCollection services)
    {
        services.AddScoped<ICoupleRepository, CoupleRepository>();
        services.AddScoped<ICoupleAppService, CoupleAppService>();
        services.AddScoped<CoupleService>();

        return services;
    }
}