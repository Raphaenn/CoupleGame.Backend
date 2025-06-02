using Domain.Entities;

namespace Domain.Interfaces;

public interface IQuizRepository
{
    Task<Quiz> StartQuiz(Guid quizId, Guid coupleId, Guid questionId);
    
    Task<Quiz> UpdateQuiz(Guid quizId, string questionPosition, Guid questionId);

    Task<Quiz> GetQuizById(Guid id); 
    
    Task<Quiz?> GetQuizByCoupleId(Guid coupleId);
}