using Domain.Entities;

namespace Domain.Interfaces;

public interface IQuizRepository
{
    Task CreateQuiz(Quiz quiz);
    Task<Quiz?> GetQuizByCoupleId(Guid coupleId);
}