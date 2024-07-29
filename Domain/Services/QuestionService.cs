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

    public async Task<Question> GetSingleQuestion(Guid questionId)
    {
        Question response = await _questionRepository.GetSingleQuestion(questionId);
        return response;
    }

    public async Task<List<Question>> GetQuestionsByTopicId(Guid topicId)
    {
        List<Question> response = await _questionRepository.GetQuestionsByTopicId(topicId);
        return response;
    }
}