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

    public async Task<QuizDto?> UpdateQuiz(string quizId, string questionId)
    {
        try
        {
            Guid parsedQuizId = Guid.Parse(quizId);
            Guid parsedQuestionId = Guid.Parse(questionId);
            Quiz quiz = await _quizService.UpdateQuizQuestion(parsedQuizId, parsedQuestionId);

            if (quiz == null)
            {
                return null;
            }

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
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<QuizDto?> GetQuizByCoupleId(string coupleId)
    {
        throw new NotImplementedException();
    }

    public async Task<QuizDto> GetInviteQuiz(string quizId)
    {
        try
        {
            Guid parsedQuizId = Guid.Parse(quizId);
            Quiz quiz = await _quizService.GetQuizById(parsedQuizId);
            QuizDto parsedQuiz = new QuizDto
            {
                QuizId = quiz.Id.ToString(),
                CoupleId = quiz.CoupleId.ToString(),
                QuestionId1 = quiz.Question1.ToString(),
                QuestionId2 = quiz.Question2.ToString(),
                QuestionId3 = quiz.Question3.ToString(),
                QuestionId4 = quiz.Question4.ToString(),
                QuestionId5 = quiz.Question5.ToString(),
                QuestionId6 = quiz.Question6.ToString(),
                CreatedAt = quiz.CreatedAt,
                Questions = quiz.QuestionsList.Select(q => new QuestionDto()
                {
                    Id = q.Id.ToString(),
                    TopicId = q.TopicId.ToString(),
                    QuestionText = q.QuestionText,
                    Answer1 = q.Answer1,
                    Answer2 = q.Answer2,
                    Answer3 = q.Answer3,
                    Answer4 = q.Answer4
                }).ToList()
            };
            return parsedQuiz;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<AnswerDto> AnswerQuizQuestion(string userId, string quizId, string answer)
    {
        try
        {
            Guid parsedUserId = Guid.Parse(userId);
            Guid parsedQuizId = Guid.Parse(quizId);
            Answers answers = await _quizService.AnswerAQuizQuest(parsedUserId, parsedQuizId, answer);

            AnswerDto answerDto = new AnswerDto 
            {
                Id = answers.Id,
                UserId = answers.UserId,
                Answer1 = answers.Answer1,
                Answer2 = answers.Answer2,
                Answer3 = answers.Answer3,
                Answer4 = answers.Answer4,
                Answer5 = answers.Answer5,
                Answer6 = answers.Answer6,
                CreatedAt = answers.CreatedAt
            };

            return answerDto;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<QuizDto> CompletedQuiz(string quizId)
    {
        try
        {
            Guid parsedId = Guid.Parse(quizId);
            Quiz quiz = await _quizService.GetCompletedQuiz(parsedId);
            
            QuizDto parsedQuiz = new QuizDto
            {
                QuizId = quiz.Id.ToString(),
                CoupleId = quiz.CoupleId.ToString(),
                QuestionId1 = quiz.Question1.ToString(),
                QuestionId2 = quiz.Question2.ToString(),
                QuestionId3 = quiz.Question3.ToString(),
                QuestionId4 = quiz.Question4.ToString(),
                QuestionId5 = quiz.Question5.ToString(),
                QuestionId6 = quiz.Question6.ToString(),
                CreatedAt = quiz.CreatedAt,
                Questions = quiz.QuestionsList.Select(q => new QuestionDto()
                {
                    Id = q.Id.ToString(),
                    TopicId = q.TopicId.ToString(),
                    QuestionText = q.QuestionText,
                    Answer1 = q.Answer1,
                    Answer2 = q.Answer2,
                    Answer3 = q.Answer3,
                    Answer4 = q.Answer4
                }).ToList(),
                Answer = quiz.AnswersList.Select(a => new AnswerDto
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    Answer1 = a.Answer1,
                    Answer2 = a.Answer2,
                    Answer3 = a.Answer3,
                    Answer4 = a.Answer4,
                    Answer5 = a.Answer5,
                    Answer6 = a.Answer6,
                    CreatedAt = a.CreatedAt
                })
            };

            return parsedQuiz;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}