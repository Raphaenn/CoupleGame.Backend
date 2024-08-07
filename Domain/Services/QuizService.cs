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

    public async Task<Quiz> StartQuiz(string quizId, string coupleId, string questionId)
    {
        return await _quizRepository.StartQuiz(quizId, coupleId, questionId);
    }

    public async Task<Quiz> UpdateStartedQuiz(string quizId, string questionPosition, string questionId)
    {
        return await _quizRepository.UpdateStartedQuiz(quizId, questionPosition, questionId);
    }

    public async Task<Quiz> GetQuizById(string id)
    {
        return await _quizRepository.GetQuizById(id);
    }

    public async Task CreateQuiz(Quiz quiz)
    {
        await _quizRepository.CreateQuiz(quiz);
    }

    public async Task<Quiz?> GetQuizByCoupleId(string coupleId)
    {
        return await _quizRepository.GetQuizByCoupleId(coupleId);
    }
}