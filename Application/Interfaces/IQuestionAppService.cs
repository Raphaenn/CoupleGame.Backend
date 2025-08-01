using Application.Dtos;

namespace Application.Interfaces;

public interface IQuestionAppService
{
    Task<QuestionDto> GetQuestion(string questionId);
    
    Task<List<QuestionDto>> GetQuestionsByTopic(string topicId);

    Task<QuestionDto> RandomQuestion(string topicId, string quizId);
}