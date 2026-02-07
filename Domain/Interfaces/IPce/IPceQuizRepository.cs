using Domain.Entities;

namespace Domain.Interfaces.IPce;

public interface IPceQuizRepository
{
    Task CreatePremiumQuiz(Pce quiz);
    
    Task<Pce> GetPremiumQuizByCouple(Guid coupleId);
}