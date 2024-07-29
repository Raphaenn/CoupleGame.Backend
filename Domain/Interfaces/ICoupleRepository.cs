using Domain.Entities;

namespace Domain.Interfaces;

public interface ICoupleRepository
{
    Task<Couple> CreateCouple(Couple customer);
    Task<Couple?> SearchCoupleRelationship(string userOneId, string userIdTwo);
    // Task UpdateCouple(string userId);
}