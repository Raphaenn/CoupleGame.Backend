using Domain.Entities;

namespace Domain.Interfaces;

public interface ITopicRepository
{
    Task<Topic?> GetTopicById(string id);

    Task<List<Topic>> ListAllTopics();
}