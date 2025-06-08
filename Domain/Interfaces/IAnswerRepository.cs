using Domain.Entities;

namespace Domain.Interfaces;

public interface IAnswerRepository
{
    Task<Answers> GetAnswer(Guid id);
    
    Task<Answers?> GetAnswerByQuizId(Guid quizId);
    
    Task CreateAnswer(Guid id, Guid userId, Guid quizId, string answer);

    Task UpdateAnswer(Guid id, string answerPosition, string answer);
}