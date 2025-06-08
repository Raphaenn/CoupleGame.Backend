using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Services;

public class AnswerService
{
    private readonly IAnswerRepository _answerRepository;

    public AnswerService(IAnswerRepository answerRepository)
    {
        _answerRepository = answerRepository;
    }

    public async Task<Answers> GetAnswer(Guid id)
    {
        Answers response = await _answerRepository.GetAnswer(id);
        return response;
    }

    public async Task CreateAnswer(Guid id, Guid userId, Guid quizId, string answer)
    {
        await _answerRepository.CreateAnswer(id, userId, quizId, answer);
        return;
    }

    public async Task UpdateAnswer(Guid id, string answerPosition, string answer)
    {
        await _answerRepository.UpdateAnswer(id, answerPosition, answer);
        return;
    }
}