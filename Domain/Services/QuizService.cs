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