using Application.Dtos;

namespace Application.Interfaces;

public interface IQuizAppService
{
    Task CreateNewQuiz(QuizDto quizData);

    Task<QuizDto?> GetQuizByCouple(string coupleId);
}