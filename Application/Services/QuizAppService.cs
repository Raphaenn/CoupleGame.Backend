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

    public async Task<QuizDto> Start(string coupleId, string questionId)
    {
        try
        {
            string quizId = Guid.NewGuid().ToString();
            string parsedCoupleId = coupleId;
            string parsedQuestionId = questionId;
            Quiz getQuiz = await _quizRepository.StartQuiz(quizId, parsedCoupleId, parsedQuestionId);
            QuizDto response = new QuizDto
            {
                CoupleId = getQuiz.CoupleId.ToString(),
                QuestionId1 = getQuiz.QuestionId1.ToString(),
                CreatedAt = getQuiz.CreatedAt
            };

            return response;
        }
        catch
        {
            throw new Exception(message: "Create quiz error");
        }
    }

    public async Task<QuizDto> Update(string quizId, string questionId)
    {
        try
        {
            string parsedQuizId = quizId;
            string parsedQuestionId = questionId;
            Quiz getCurrentQuiz = await _quizRepository.GetQuizById(parsedQuizId);
            string questionPosition;

            string GetPosition(Quiz q)
            {
                if (q.QuestionId2 == null)
                {
                    questionPosition = "question_id_2";
                    return questionPosition;
                }
                if (q.QuestionId3 == null)
                {
                    questionPosition = "question_id_3";
                    return questionPosition;
                }
                if (q.QuestionId4 == null)
                {
                    questionPosition = "question_id_4";
                    return questionPosition;
                }
                if (q.QuestionId5 == null)
                {
                    questionPosition = "question_id_5";
                    return questionPosition;
                }
                return questionPosition = "question_id_6";;
            }

            string returnedValue = GetPosition(getCurrentQuiz);
            
            Quiz getQuiz = await _quizRepository.UpdateStartedQuiz(parsedQuizId, returnedValue, parsedQuestionId);
            
            QuizDto response = new QuizDto
            {
                CoupleId = getQuiz.CoupleId.ToString(),
                QuestionId1 = getQuiz.QuestionId1.ToString(),
                QuestionId2 = getQuiz.QuestionId2.ToString(),
                QuestionId3 = getQuiz.QuestionId3.ToString(),
                QuestionId4 = getQuiz.QuestionId4.ToString(),
                QuestionId5 = getQuiz.QuestionId5.ToString(),
                QuestionId6 = getQuiz.QuestionId6.ToString(),
                CreatedAt = getQuiz.CreatedAt
            };

            return response;
        }
        catch (Exception e)
        {
            throw new Exception(message: "Create quiz error");
        }
    }
    
    public async Task CreateNewQuiz(QuizDto quizData)
    {
        try
        {
            string quizId = Guid.NewGuid().ToString();
            Quiz newQuiz = new Quiz
            {
                Id = quizId,
                CoupleId = quizData.CoupleId,
                QuestionId1 = quizData.QuestionId1,
                QuestionId2 = quizData.QuestionId2,
                QuestionId3 = quizData.QuestionId3,
                QuestionId4 = quizData.QuestionId4,
                QuestionId5 = quizData.QuestionId5,
                QuestionId6 = quizData.QuestionId5,
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
            string parsedId = coupleId;
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