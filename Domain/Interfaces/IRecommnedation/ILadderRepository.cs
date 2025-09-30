using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Interfaces.IRecommnedation;

public interface ILadderRepository
{
    Task<Ladder?> GetLadder(LadderId id, CancellationToken ct);
}