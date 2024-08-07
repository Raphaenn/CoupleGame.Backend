using Domain.Entities;

namespace Domain.Interfaces;

public interface IAnswerRepository
{
    Task<Answers> GetAnswer(string id);
    
    Task CreateAnswer(string id, string userId, string quizId, string answer);

    Task UpdateAnswer(string id, string answerPosition, string answer);
}