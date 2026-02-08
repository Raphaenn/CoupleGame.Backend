using Domain.Entities;

namespace Domain.Interfaces.IPce;

public interface IPceAnswersRepository
{
    Task CreatePceAnswer(PceAnswer answer, CancellationToken ct);
    
    Task<List<PceAnswer>> ListPceAnswer(Guid userId, CancellationToken ct);
}