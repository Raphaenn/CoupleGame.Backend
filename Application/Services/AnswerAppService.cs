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
    
    public async Task<AnswerDto> GetAnswerById(string id)
    {
        throw new NotImplementedException();
    }

    public async Task CreateNewAnswer(string userId, string quizId, string answer)
    {
        try
        {
            Guid parsedUserId = Guid.Parse(userId);
            Guid parsedQuizId = Guid.Parse(quizId);
            Answers answers = Answers.StartAnswer(parsedUserId, parsedQuizId, answer, null, null, null, null, null);
            await _answerRepository.CreateAnswer(answers);
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