using Domain.Entities;

namespace Domain.Interfaces;

public interface ITopicRepository
{
    Task<Topic?> GetTopicById(Guid id);

    Task<List<Topic>> ListAllTopics();

    Task<Topic> GetRandomQuestionByTopic(Guid id);
}