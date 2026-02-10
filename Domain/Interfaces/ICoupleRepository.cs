using Domain.Entities;

namespace Domain.Interfaces;

public interface ICoupleRepository
{
    Task<Couple> StartNewCouple(Couple customer);
    
    Task AddCoupleMember(Guid coupleId, Guid userId);

    Task<Couple> SearchCoupleById(Guid coupleId);
    
    Task<List<Couple>> SearchCoupleByUserId(Guid userId);
    
    Task<Couple?> SearchCoupleRelationship(string userOneId, string userIdTwo);
    
    Task<Couple?> SearchTemCouple(Guid userOneId, Guid userIdTwo);

    Task<Couple?> GetLongTermCouple(Guid userId);
}