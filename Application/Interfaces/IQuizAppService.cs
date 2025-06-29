using Application.Dtos;

namespace Application.Interfaces;

public interface IQuizAppService
{
    Task<QuizDto> StartQuiz(string coupleId, string questionId);
    
    Task<QuizDto?> UpdateQuiz(string quizId, string questionId);

    Task<QuizDto?> GetQuizByCoupleId(string coupleId);

    Task<QuizDto> GetInviteQuiz(string quizId);

    Task<AnswerDto> AnswerQuizQuestion(string userId, string quizId, string answer);

    Task<QuizDto> CompletedQuiz(string quizId);
    
    
    
    // Task<Quiz> StartQuiz(Guid quizId, Guid coupleId, Guid questionId);
    //
    // Task UpdateQuiz(Quiz quiz);
    //
    // Task<Quiz> GetQuizById(Guid id); 
    //
    // Task<Quiz?> GetQuizByCoupleId(Guid coupleId);
}