using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Services;

public class QuestionService : IQuestionRepository
{
    private readonly IQuestionRepository _questionRepository;

    public QuestionService(IQuestionRepository questionRepository)
    {
        _questionRepository = questionRepository;
    }

    public async Task<Question> GetSingleQuestion(string questionId)
    {
        return await _questionRepository.GetSingleQuestion(questionId);
    }

    public async Task<List<Question>> GetQuestionsByTopicId(string topicId)
    {
        return await _questionRepository.GetQuestionsByTopicId(topicId);
    }
}