using Application.Dtos;
using Domain.Entities;
using Domain.Services;
using Domain.ValueObjects;

namespace Application.Interfaces;

public interface IRecommendationAppService
{
    Task<CursorPage<UserDto>> GetRecommendationService(int size, RankingCursor? after, CancellationToken ct);
    
    Task<IEnumerable<PersonRating>> SimulateRecommendationService(LadderId ladderId);

    Task<Ladder> GetLadderById(string id, CancellationToken ct);

    Task RecordVoteService(LadderId ladderId, Guid a, Guid b, InteractionType interaction, string? idempotencyKey, CancellationToken ct);

    Task? ShowRanking(LadderId ladderId);

    // Task<(Guid A, Guid B)> GetPairAsync(LadderId ladderId, CancellationToken ct);
}