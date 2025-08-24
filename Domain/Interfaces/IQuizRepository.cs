using Domain.Entities;

namespace Domain.Interfaces;

public interface IQuizRepository
{
    Task<Quiz> StartQuiz(Guid quizId, Guid coupleId, Guid questionId);
    
    Task UpdateQuiz(Quiz quiz);

    Task<Quiz> GetQuizById(Guid id); 
    
    Task<Quiz?> GetQuizByCoupleId(Guid coupleId);

    Task ChangeQuizStatus(Guid quizId, QuizStatus status);
}