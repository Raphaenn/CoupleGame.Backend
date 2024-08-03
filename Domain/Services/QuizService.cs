using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Services;

public class QuizService : IQuizRepository
{
    private readonly IQuizRepository _quizRepository;

    public QuizService(IQuizRepository quizRepostiory)
    {
        _quizRepository = quizRepostiory;
    }

    public async Task<Quiz> StartQuiz(Guid quizId, Guid coupleId, Guid questionId)
    {
        Quiz response = await _quizRepository.StartQuiz(quizId, coupleId, questionId);
        return response;
    }

    public async Task<Quiz> UpdateStartedQuiz(Guid quizId, string questionPosition, Guid questionId)
    {
        Quiz response = await _quizRepository.UpdateStartedQuiz(quizId, questionPosition, questionId);
        return response;
    }

    public async Task<Quiz> GetQuizById(Guid id)
    {
        Quiz response = await _quizRepository.GetQuizById(id);
        return response;
    }

    public async Task CreateQuiz(Quiz quiz)
    {
        await _quizRepository.CreateQuiz(quiz);
    }

    public async Task<Quiz?> GetQuizByCoupleId(Guid coupleId)
    {
        Quiz? response = await _quizRepository.GetQuizByCoupleId(coupleId);

        if (response == null)
        {
            return null;
        }

        return response;
    }
}