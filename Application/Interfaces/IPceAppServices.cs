using Application.Dtos;

namespace Application.Interfaces;

public interface IPceAppServices
{
    Task<PceDto> InitNewPce(Guid coupleId, CancellationToken ct);
    
    Task<PceDto?> GetPceByCouple(Guid coupleId, CancellationToken ct);

    Task SaveAnswers(Guid userId, Guid quizId, Guid questionId, Guid topicId, string content, CancellationToken ct);

    Task<List<PceResultDto>> GetPceResult(Guid pceId, CancellationToken ct);

    Task DeleteCompletePce(Guid pceId, CancellationToken ct);
}