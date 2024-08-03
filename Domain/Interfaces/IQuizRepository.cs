using Domain.Entities;

namespace Domain.Interfaces;

public interface IQuizRepository
{
    Task<Quiz> StartQuiz(Guid quizId, Guid coupleId, Guid questionId);
    
    Task<Quiz> UpdateStartedQuiz(Guid quizId, string questionPosition, Guid questionId);

    Task<Quiz> GetQuizById(Guid id); 
    
    Task CreateQuiz(Quiz quiz);
    
    Task<Quiz?> GetQuizByCoupleId(Guid coupleId);
}