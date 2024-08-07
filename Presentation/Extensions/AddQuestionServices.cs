using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Domain.Services;
using Infrastructure.Repository.Database;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation.Extensions;
public static class QuestionServiceCollectionExtensions
{
    public static IServiceCollection AddQuestionServices(this IServiceCollection services)
    {
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<IQuestionAppService, QuestionAppService>();
        services.AddScoped<QuestionService>();

        return services;
    }
}