using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Services;

namespace Application.Services;

public class QuizAppService : IQuizAppService
{
    private readonly QuizService _quizService;

    public QuizAppService(QuizService quizService)
    {
        _quizService = quizService;
    }

    public async Task<QuizDto> StartQuiz(string coupleId, string questionId)
    {
        Guid parsedCoupleId = Guid.Parse(coupleId);
        Guid parsedQuestionId = Guid.Parse(questionId);
        Quiz quiz = await _quizService.StartQuiz(parsedCoupleId, parsedQuestionId);
        
        return new QuizDto
        {
            QuizId = quiz.Id.ToString(),
            CoupleId = quiz.CoupleId.ToString(),
            QuestionId1 = quiz.Question1.ToString(),
            QuestionId2 = quiz.Question2.ToString(),
            QuestionId3 = quiz.Question3.ToString(),
            QuestionId4 = quiz.Question4.ToString(),
            QuestionId5 = quiz.Question5.ToString(),
            QuestionId6 = quiz.Question6.ToString(),
            CreatedAt = quiz.CreatedAt
        };
    }

    public async Task<QuizDto> UpdateQuiz(string quizId, string questionId)
    {
        throw new NotImplementedException();
    }

    public async Task<QuizDto?> GetQuizByCoupleId(string coupleId)
    {
        throw new NotImplementedException();
    }
}