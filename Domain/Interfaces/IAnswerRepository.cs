using Domain.Entities;

namespace Domain.Interfaces;

public interface IAnswerRepository
{
    Task<Answers> GetAnswer(Guid id);
    
    Task<Answers> GetAnswerByQuizId(Guid? quizId);
    
    Task CreateAnswer(Answers answer);

    Task UpdateAnswer(Guid id, string answerPosition, string answer);

    Task<Answers?> GetCompletedAnswers(Guid quizId, Guid userId);

    Task<List<Answers>> ListAnswersByQuizId(Guid quizId);
}