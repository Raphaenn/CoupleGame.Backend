using Domain.Entities;

namespace Domain.Interfaces.IPce;

public interface IPceRepository
{
    Task CreatePce(Pce pce, CancellationToken ct);
    
    Task<Pce?> GetPceById(Guid pceId, CancellationToken ct);
    
    Task<Pce?> GetPceByCouple(Guid coupleId, CancellationToken ct);

    Task DeletePceAndData(Guid pceId, CancellationToken ct);
    
    Task UpdatePceStatus(Pce pce, CancellationToken ct);
}