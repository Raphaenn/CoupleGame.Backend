using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Services;

public class AnswerService
{
    public Answers GetAnswer(Guid id)
    {
        throw new NotImplementedException();
    }

    public void CreateAnswer(Guid id, Guid userId, Guid quizId, string answer)
    {
        throw new NotImplementedException();
    }

    public void UpdateAnswer(Guid id, string answerPosition, string answer)
    {
        throw new NotImplementedException();
    }
}