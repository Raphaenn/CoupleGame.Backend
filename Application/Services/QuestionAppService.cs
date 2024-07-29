using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class QuestionAppService : IQuestionAppService
{
    private readonly IQuestionRepository _questionRepository;

    public QuestionAppService(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }
    
    public async Task<QuestionDto> GetQuestion(string questionId)
    {
        try
        {
            Guid parsedId = Guid.Parse(questionId);
            Question quest = await _questionRepository.GetSingleQuestion(parsedId);
            QuestionDto parsedQuest = new QuestionDto
            {
                Id = quest.Id.ToString(),
                TopicId = quest.TopicId.ToString(),
                QuestionText = quest.QuestionText,
                Answer1 = quest.Answer1,
                Answer2 = quest.Answer2,
                Answer3 = quest.Answer3,
                Answer4 = quest.Answer4
            };

            return parsedQuest;
        }
        catch (Exception e)
        {
            throw new Exception(message: e.Message);
        }
    }

    public async Task<List<QuestionDto>> GetQuestionsByTopic(string topicId)
    {
        try
        {
            Guid parsedId = Guid.Parse(topicId);
            List<Question> quests = await _questionRepository.GetQuestionsByTopicId(parsedId);

            List<QuestionDto> questList = new List<QuestionDto>();

            foreach (var q in quests)
            {
                QuestionDto question = new QuestionDto
                {
                    Id = q.Id.ToString(),
                    TopicId = q.TopicId.ToString(),
                    QuestionText = q.QuestionText,
                    Answer1 = q.Answer1,
                    Answer2 = q.Answer2,
                    Answer3 = q.Answer3,
                    Answer4 = q.Answer4
                };
                questList.Add(question);
            }
            return questList;
        }
        catch (Exception e)
        {
            throw new Exception(message: e.Message);
        }
    }
}