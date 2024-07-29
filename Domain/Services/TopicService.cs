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

    public async Task<Topic?> GetTopicById(Guid id)
    {
        Topic? response = await _topicRepository.GetTopicById(id);

        if (response == null)
        {
            return null;
        }
        return response;
    }

    public async Task<List<Topic>> ListAllTopics()
    {
        List<Topic> response = await _topicRepository.ListAllTopics();
        return response;
    }
}