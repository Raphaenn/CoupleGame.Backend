using Domain.ValueObjects;

namespace Domain.Interfaces.IRecommnedation;

public interface IRatingHistoryRepository
{
    Task InsertAsync(LadderId
            ladderId, Guid userId, Guid matchId,
        double before, double after, short result, DateTimeOffset at,
        CancellationToken ct);
}