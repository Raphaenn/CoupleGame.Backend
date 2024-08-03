using Application.Dtos;

namespace Application.Interfaces;

public interface IAnswerAppService
{
    Task<AnswerDto> GetAnswerById(string id);
    
    Task CreateNewAnswer(string userId, string quizId, string answer);

    Task UpdateAnswerById(string id, string answer);
}