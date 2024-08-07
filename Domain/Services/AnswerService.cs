using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Services;

public class AnswerService : IAnswerRepository
{
    private readonly IAnswerRepository _answerRepository;

    public AnswerService(IAnswerRepository answerRepository)
    {
        _answerRepository = answerRepository;
    }

    public async Task<Answers> GetAnswer(string id)
    {
        return await _answerRepository.GetAnswer(id);
    }

    public async Task CreateAnswer(string id, string userId, string quizId, string answer)
    {
        await _answerRepository.CreateAnswer(id, userId, quizId, answer);
    }

    public async Task UpdateAnswer(string id, string answerPosition, string answer)
    {
        await _answerRepository.UpdateAnswer(id, answerPosition, answer);
    }
}