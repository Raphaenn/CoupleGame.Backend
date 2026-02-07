using Domain.Entities;

namespace Domain.Interfaces.IPCE;

public interface IPceEAnswersRepository
{
    Task CreatePremiumAnswer(PceAnswer answer, CancellationToken ct);
    
    Task<List<PceAnswer>> ListPremiumAnswerByTopic(Guid userId, Guid questionId, CancellationToken ct);
}