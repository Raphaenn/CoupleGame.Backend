using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Services;

namespace Application.Services;

public class TopicAppService : ITopicAppService
{
    private readonly ITopicRepository _topicRepository;
    private readonly IQuestionRepository _questionRepository;

    public TopicAppService(ITopicRepository topicRepository, IQuestionRepository questionRepository)
    {
        this._topicRepository = topicRepository;
        this._questionRepository = questionRepository;
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

    public async Task<TopicDto> GetQuestionByTopicId(string topicId)
    {
        try
        {
            Guid parsedId = Guid.Parse(topicId);
            var topic = await _topicRepository.GetTopicById(parsedId);
             List<Question> questionList = await _questionRepository.GetQuestionsByTopicId(parsedId);

             if (topic == null)
                 throw new InvalidOperationException("Topic not found");

             if (!topic.Status)
                 throw new InvalidOperationException("Invalid topic status");
             
             questionList.ForEach(q => topic.AddQuestion(q));

             var question = topic.GetRandomQuestion();

            TopicDto parsedTopic = new TopicDto
            {
                Id = topic.Id.ToString(),
                Name = topic.Name,
                Description = topic.Description,
                Status = topic.Status,
                Questions = question
            };
            return parsedTopic;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
    }
}