using Domain.Entities;

namespace Domain.Interfaces.IPce;

public interface IPceRepository
{
    Task CreatePce(Pce quiz, CancellationToken ct);
    
    Task<Pce?> GetPceByCouple(Guid coupleId, CancellationToken ct);
}