using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Services;

public class TopicService
{
    private readonly ITopicRepository _topicRepository;
    private readonly IQuestionRepository _questionRepository;

    public TopicService(ITopicRepository topicRepository, IQuestionRepository questionRepository)
    {
        _topicRepository = topicRepository;
        _questionRepository = questionRepository;
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
    
    public async Task<(Topic Topic, Question Question)> GetTopicWithRandomQuestion(Guid topicId)
    {
        var topic = await _topicRepository.GetTopicById(topicId);
        List<Question> questionList = await _questionRepository.GetQuestionsByTopicId(topicId);

        if (topic == null)
            throw new InvalidOperationException("Topic not found");

        if (!topic.Status)
            throw new InvalidOperationException("Invalid topic status");
        
        questionList.ForEach(q => topic.AddQuestion(q));

        var question = topic.GetRandomQuestion();

        return (topic, question);
    }
}