using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Interfaces.IRecommnedation;

public interface IParticipantRatingRepository
{
    Task EnsureExistsAsync(LadderId ladderId, Guid userId, CancellationToken ct);
    Task<PersonRating> GetForUpdateAsync(LadderId ladderId, Guid userId, CancellationToken ct);
    Task UpdateAsync(PersonRating rating, CancellationToken ct);
}