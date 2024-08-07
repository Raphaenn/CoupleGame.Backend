using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Services;

public class TopicService : ITopicRepository
{
    private readonly ITopicRepository _topicRepository;

    public TopicService(ITopicRepository topicRepository)
    {
        _topicRepository = topicRepository;
    }

    public async Task<Topic?> GetTopicById(string id)
    {
        return await _topicRepository.GetTopicById(id);
    }

    public async Task<List<Topic>> ListAllTopics()
    {
        List<Topic> response = await _topicRepository.ListAllTopics();
        return response;
    }
}