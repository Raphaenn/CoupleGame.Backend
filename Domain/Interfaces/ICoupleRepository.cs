using Domain.Entities;

namespace Domain.Interfaces;

public interface ICoupleRepository
{
    Task<Couple> StartNewCouple(Couple customer);
    Task AddCoupleMember(Guid coupleId, Guid userId);

    Task<Couple> SearchCoupleById(Guid coupleId);
    Task<Couple> SearchCoupleByUserId(Guid userId);
    Task<Couple?> SearchCoupleRelationship(string userOneId, string userIdTwo);
}