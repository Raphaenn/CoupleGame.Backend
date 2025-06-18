using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Domain.Services;
using Infrastructure.Repository.Database;

namespace Api.Extensions;

public static class QuizServiceCollectionExtensions
{
    public static IServiceCollection AddQuizServices(this IServiceCollection services)
    {
        services.AddScoped<IQuizRepository, QuizRepository>();
        services.AddScoped<IQuizAppService, QuizAppService>();
        
        return services;
    }
}