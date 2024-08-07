using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Domain.Services;
using Infrastructure.Repository.Database;

namespace Presentation.Extensions;

public static class AddAnswerCollectionExtensions
{
    public static IServiceCollection AddAnswerServices(this IServiceCollection services)
    {
        services.AddScoped<IAnswerRepository, AnswerRepository>();
        services.AddScoped<IAnswerAppService, AnswerAppService>();
        services.AddScoped<AnswerService>();

        return services;
    }
}