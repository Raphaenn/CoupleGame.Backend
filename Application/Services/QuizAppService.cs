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
            Guid quizId = Guid.NewGuid();
            Guid parsedCoupleId = Guid.Parse(coupleId);
            Guid parsedQuestionId = Guid.Parse(questionId);
            Quiz getQuiz = await _quizRepository.StartQuiz(quizId, parsedCoupleId, parsedQuestionId);
            QuizDto response = new QuizDto
            {
                CoupleId = getQuiz.CoupleId.ToString(),
                QuestionId1 = getQuiz.QuestionId1.ToString(),
                CreatedAt = getQuiz.CreatedAt
            };

            return response;
        }
        catch (Exception e)
        {
            throw new Exception(message: "Create quiz error");
        }
    }

    // private async Task<QuizDto> GetById(string id)
    // {
    //     try
    //     {
    //         Guid parsedId = Guid.Parse(id);
    //         Quiz quiz = await _quizRepository.GetQuizById(parsedId);
    //         QuizDto parsedQuiz = new QuizDto
    //         {
    //             CoupleId = quiz.CoupleId.ToString(),
    //             QuestionId1 = quiz.QuestionId1.ToString(),
    //             QuestionId2 = quiz.QuestionId2.ToString(),
    //             QuestionId3 = quiz.QuestionId3.ToString(),
    //             QuestionId4 = quiz.QuestionId4.ToString(),
    //             QuestionId5 = quiz.QuestionId5.ToString(),
    //             QuestionId6 = quiz.QuestionId6.ToString(),
    //             CreatedAt = quiz.CreatedAt
    //         };
    //
    //         return parsedQuiz;
    //     }
    //     catch (Exception e)
    //     {
    //         throw new Exception(e.Message);
    //     }
    // }

    public async Task<QuizDto> Update(string quizId, string questionId)
    {
        try
        {
            Guid parsedQuizId = Guid.Parse(quizId);
            Guid parsedQuestionId = Guid.Parse(questionId);
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