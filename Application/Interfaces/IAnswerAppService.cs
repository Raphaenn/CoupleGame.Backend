using Application.Dtos;

namespace Application.Interfaces;

public interface IAnswerAppService
{
    Task<CompletedAnswers> GetCompleteAnswerByQuiz(string quiz, string userId);

    Task<AnswerDto> AnswerQuiz(string userId, string quizId, string answer1, string answer2, string answer3, string answer4, string answer5, string answer6);

    Task UpdateAnswerById(string id, string answer);
}