using Domain.Entities;

namespace Domain.Interfaces;

public interface IQuestionRepository
{
    Task<Question> GetSingleQuestion(Guid questionId);

    Task<List<Question>> GetQuestionsByTopicId(Guid topicId);
}