using Domain.Entities;

namespace Domain.Interfaces;

public interface IQuestionRepository
{
    Task<Question> GetSingleQuestion(string questionId);

    Task<List<Question>> GetQuestionsByTopicId(string topicId);
}