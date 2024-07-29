using Application.Dtos;

namespace Application.Interfaces;

public interface ITopicAppService
{
    Task<TopicDto?> GetTopicById(string id);

    Task<List<TopicDto>> ListAllTopics();
}