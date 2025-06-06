using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Services;

public class QuizService
{
    private readonly IQuizRepository _quizRepository;

    public QuizService(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<Quiz> StartQuiz(Guid coupleId, Guid questionId)
    {
        // Couples are allowed only one active quiz at a time.
        Quiz? checkByCouple = await _quizRepository.GetQuizByCoupleId(coupleId);

        if (checkByCouple != null)
        {
            throw new Exception("Couples are allowed only one active quiz at a time");
        }

        Quiz quiz = Quiz.StartQuiz(coupleId, questionId);
        
        Quiz response = await _quizRepository.StartQuiz(quiz.Id, quiz.CoupleId, quiz.Question1);
        return response;
    }

    public async Task<Quiz> UpdateQuizQuestion(Guid quizId, Guid questionId)
    {
        Quiz quiz = await _quizRepository.GetQuizById(quizId);
        
        if (quiz == null)
            throw new Exception("Quiz not found");

        bool added = quiz.Update(questionId);

        Console.WriteLine(quiz.Id);
        await _quizRepository.UpdateQuiz(quiz);

        return added ? quiz : null;
    }

    public async Task<Quiz> GetQuizById(Guid id)
    {
        Quiz response = await _quizRepository.GetQuizById(id);
        return response;
    }

    public async Task<Quiz?> GetQuizByCoupleId(Guid coupleId)
    {
        Quiz? response = await _quizRepository.GetQuizByCoupleId(coupleId);
        return response;
    }
}