using Application.Dtos;
using Domain.Entities;

namespace Application.Interfaces;

public interface IQuizAppService
{
    Task<QuizDto> StartQuiz(string coupleId, string questionId);
    
    Task<QuizDto?> UpdateQuiz(string quizId, string questionId);

    Task<List<QuizDto>> ListOpenQuiz(string userId);

    Task<QuizDto> GetInviteQuiz(string quizId);

    Task<AnswerDto> AnswerQuizQuestion(string userId, string quizId, string answer);

    Task<QuizDto> GetResult(string quizId);

    Task<QuizDto> UpdateQuizStatus(string quizId, string status);
}