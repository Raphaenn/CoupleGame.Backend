using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class TopicAppService : ITopicAppService
{
    private readonly ITopicRepository _topicRepository;

    public TopicAppService(ITopicRepository topicRepository)
    {
        this._topicRepository = topicRepository;
    }
    
    public async Task<TopicDto?> GetTopicById(string id)
    {
        try
        {
            Guid parsedId = Guid.Parse(id);
            Topic? geTopic = await _topicRepository.GetTopicById(parsedId);

            if (geTopic == null)
            {
                return null;
            }
            TopicDto mapperTopic = new TopicDto
            {
                Name = geTopic.Name,
                Description = geTopic.Description,
                Status = geTopic.Status
            };
            return mapperTopic;
        }
        catch (Exception e)
        {
            throw new Exception(message: e.Message);
        }
    }

    public async Task<List<TopicDto>> ListAllTopics()
    {
        try
        {
            List<Topic> allTopics = await _topicRepository.ListAllTopics();
            List<TopicDto> list = new List<TopicDto>();

            foreach (Topic t in allTopics)
            {
                TopicDto topic = new TopicDto
                {
                    Id = t.Id.ToString(),
                    Name = t.Name,
                    Description = t.Description,
                    Status = t.Status
                };
                if (list == null)
                {
                    throw new ArgumentNullException(nameof(list));
                }

                list.Add(topic);
            }

            return list;
        }
        catch (Exception e)
        {
            throw new Exception(message: e.Message);
        }
    }
}