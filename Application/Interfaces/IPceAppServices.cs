using Application.Dtos;

namespace Application.Interfaces;

public interface IPceAppServices
{
    Task InitNewPce(Guid coupleId, CancellationToken ct);
    
    Task SaveAnswers(Guid userId, Guid quizId, Guid questionId, Guid topicId, string content, CancellationToken ct);

    Task<PceDto?> GetPceByCouple(Guid coupleId, CancellationToken ct);

    Task<List<PceResultDto>> GetPceResult(Guid coupleId, CancellationToken ct);
}