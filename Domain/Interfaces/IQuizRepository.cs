using Domain.Entities;

namespace Domain.Interfaces;

public interface IQuizRepository
{
    Task<Quiz> StartQuiz(string quizId, string coupleId, string questionId);
    
    Task<Quiz> UpdateStartedQuiz(string quizId, string questionPosition, string questionId);

    Task<Quiz> GetQuizById(string id); 
    
    Task CreateQuiz(Quiz quiz);
    
    Task<Quiz?> GetQuizByCoupleId(string coupleId);
}