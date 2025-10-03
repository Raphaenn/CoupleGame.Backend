using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Interfaces;

public interface IRecommendationAppService
{
    Task<IEnumerable<PersonRating>> GetRecommendationService(LadderId ladderId);

    Task<Ladder> GetLadderById(string id, CancellationToken ct);

    Task RecordVoteService(LadderId ladderId, Guid a, Guid b, Guid winner, string? idempotencyKey, CancellationToken ct);

    Task? ShowRanking(LadderId ladderId);

    // Task<(Guid A, Guid B)> GetPairAsync(LadderId ladderId, CancellationToken ct);
}