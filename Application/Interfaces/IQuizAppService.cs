using Application.Dtos;

namespace Application.Interfaces;

public interface IQuizAppService
{
    Task<QuizDto> Start(string coupleId, string questionId);
    
    Task<QuizDto> Update(string quizId, string questionId);

    Task CreateNewQuiz(QuizDto quizData);

    Task<QuizDto?> GetQuizByCouple(string coupleId);
}