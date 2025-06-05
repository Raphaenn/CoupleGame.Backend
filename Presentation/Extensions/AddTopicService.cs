using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Domain.Services;
using Infrastructure.Repository.Database;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Extensions;
public static class TopicServiceCollectionExtensions
{
    public static IServiceCollection AddTopicService(this IServiceCollection services)
    {
        services.AddScoped<ITopicRepository, TopicRepository>();
        services.AddScoped<ITopicRepository, TopicRepository>();
        services.AddScoped<ITopicAppService, TopicAppService>();
        services.AddScoped<TopicService>();
        
        return services;
    }
}