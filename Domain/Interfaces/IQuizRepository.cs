using Domain.Entities;

namespace Domain.Interfaces;

public interface IQuizRepository
{
    Task<Quiz> StartQuiz(Guid quizId, Guid coupleId, Guid questionId, QuizStatus status);
    
    Task UpdateQuiz(Quiz quiz);

    Task<Quiz> GetQuizById(Guid id); 
    
    Task<Quiz?> GetQuizByCoupleId(Guid coupleId);
    
    Task<List<Quiz>> ListQuizByCoupleId(Guid coupleId);

    Task ChangeQuizStatus(Guid quizId, QuizStatus status);

    Task<List<Quiz>> ListCompletedQuizzesByCoupleId(Guid coupleId);
}