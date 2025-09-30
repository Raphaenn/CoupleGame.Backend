using Domain.Entities;

namespace Domain.Interfaces.IRecommnedation;

public interface IMatchVoteRepository
{
    Task<Guid> InsertAsync(MatchVote vote, CancellationToken ct);
    Task<Guid?> GetByIdempotencyKeyAsync(string key, CancellationToken ct);
}