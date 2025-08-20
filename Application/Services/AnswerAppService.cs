using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class AnswerAppService : IAnswerAppService
{
    private readonly IAnswerRepository _answerRepository;

    public AnswerAppService(IAnswerRepository answerRepository)
    {
        _answerRepository = answerRepository;
    }
    
    public async Task<CompletedAnswers> GetCompleteAnswerByQuiz(string quizId, string userId)
    {
        try
        {
            Guid parsedQuizId = Guid.Parse(quizId);
            Guid parsedUserId = Guid.Parse(userId);
            Answers? answers = await _answerRepository.GetCompletedAnswers(parsedQuizId, parsedUserId);

            if (answers == null)
            {
                throw new ApplicationException("No completed answers was found");
            }
            
            CompletedAnswers res = new CompletedAnswers
            {
                Id = answers.Id.ToString(),
                UserId = answers.UserId.ToString(),
                QuizId = answers.QuizId.ToString(),
                Question1 = answers.Questions[0],
                Answer1 = answers.Answer1,
                Question2 = answers.Questions[1],
                Answer2 = answers.Answer2,
                Question3 = answers.Questions[2],
                Answer3 = answers.Answer3,
                Question4 = answers.Questions[3],
                Answer4 = answers.Answer4,
                Question5 = answers.Questions[4],
                Answer5 = answers.Answer5,
                Question6 = answers.Questions[5],
                Answer6 = answers.Answer6,
                CreatedAt = answers.CreatedAt
            };

            return res;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<AnswerDto> AnswerQuiz(string userId, string quizId, string answer1, string answer2, string answer3, string answer4, string answer5, string answer6)
    {
        try
        {
            Guid parsedUserId = Guid.Parse(userId);
            Guid parsedQuizId = Guid.Parse(quizId);
            Answers answers = Answers.StartAnswer(
                parsedUserId, 
                parsedQuizId,
                answer1,
                answer2,
                answer3,
                answer4,
                answer5,
                answer6
            );
            
            await _answerRepository.CreateAnswer(answers);
            AnswerDto parsed = new AnswerDto
            {
                Id = answers.Id.ToString(),
                UserId = answers.UserId.ToString(),
                QuizId = answers.QuizId.ToString(),
                Answer1 = answers.Answer1,
                Answer2 = answers.Answer2,
                Answer3 = answers.Answer3,
                Answer4 = answers.Answer4,
                Answer5 = answers.Answer5,
                Answer6 = answers.Answer6,
                CreatedAt = answers.CreatedAt
            };

            return parsed;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task UpdateAnswerById(string id, string answer)
    {
        try
        {
            Guid parsedAnswerId = Guid.Parse(id);
            Answers getAnswer = await _answerRepository.GetAnswer(parsedAnswerId);
            string answerPosition;

            string GetPosition(Answers a)
            {
                if (a.Answer2 == null)
                {
                    answerPosition = "answer_2";
                    return answerPosition;
                    }
                if (a.Answer3 == null)
                {
                    answerPosition = "answer_3";
                    return answerPosition;
                }
                if (a.Answer4 == null)
                {
                    answerPosition = "answer_4";
                    return answerPosition;
                }
                if (a.Answer5 == null)
                {
                    answerPosition = "answer_5";
                    return answerPosition;
                }
                return answerPosition = "answer_6";
            }

            string returnedValue = GetPosition(getAnswer);

            await _answerRepository.UpdateAnswer(parsedAnswerId, returnedValue, answer);
        }
        catch (Exception e)
        {
            throw new Exception(message: "Create quiz error");
        }
    }
}