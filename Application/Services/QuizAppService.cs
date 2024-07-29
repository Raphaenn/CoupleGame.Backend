using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class QuizAppService : IQuizAppService
{
    private readonly IQuizRepository _quizRepository;

    public QuizAppService(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task CreateNewQuiz(QuizDto quizData)
    {
        try
        {
            Guid quizId = Guid.NewGuid();
            Quiz newQuiz = new Quiz
            {
                Id = quizId,
                CoupleId = Guid.Parse(quizData.CoupleId),
                QuestionId1 = Guid.Parse(quizData.QuestionId1),
                QuestionId2 = Guid.Parse(quizData.QuestionId2),
                QuestionId3 = Guid.Parse(quizData.QuestionId3),
                QuestionId4 = Guid.Parse(quizData.QuestionId4),
                QuestionId5 = Guid.Parse(quizData.QuestionId5),
                QuestionId6 = Guid.Parse(quizData.QuestionId5),
                CreatedAt = quizData.CreatedAt
            };

            await _quizRepository.CreateQuiz(newQuiz);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw new Exception(message: "Create quiz error");
        }
    }

    public async Task<QuizDto?> GetQuizByCouple(string coupleId)
    {
        try
        {
            Guid parsedId = Guid.Parse(coupleId);
            Quiz? getQuiz = await _quizRepository.GetQuizByCoupleId(parsedId);

            if (getQuiz == null)
            {
                return null;
            }

            QuizDto parsedResponse = new QuizDto
            {
                CoupleId = getQuiz.CoupleId.ToString(),
                QuestionId1 = getQuiz.QuestionId1.ToString(),
                QuestionId2 = getQuiz.QuestionId2.ToString(),
                QuestionId3 = getQuiz.QuestionId3.ToString(),
                QuestionId4 = getQuiz.QuestionId4.ToString(),
                QuestionId5 = getQuiz.QuestionId5.ToString(),
                QuestionId6 = getQuiz.QuestionId5.ToString(),
                CreatedAt = getQuiz.CreatedAt
            };

            return parsedResponse;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception(message: "Get quiz error!");
        }
    }
}