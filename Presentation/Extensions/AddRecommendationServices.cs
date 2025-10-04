using Application.Interfaces;
using Application.Services;
using Domain.Interfaces.IRecommnedation;
using Domain.Services;
using Infrastructure.Repository.Database;

namespace Api.Extensions;

public static class RecommendationServiceCollectionExtensions
{
    public static IServiceCollection AddRecommendationServices(this IServiceCollection services)
    {
        // services.AddScoped<IAnswerRepository, AnswerRepository>();
        services.AddScoped<ILadderRepository, RecommendationRepository>();
        services.AddScoped<IParticipantRatingRepository, RecommendationRepository>();
        services.AddScoped<IMatchVoteRepository, RecommendationRepository>();
        services.AddScoped<IRecommendationAppService, RecommendationAppService>();
        services.AddScoped<EloRatingService>();

        return services;
    }
}