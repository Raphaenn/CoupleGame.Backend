using Domain.Entities;

namespace Domain.Interfaces;

public interface ICoupleRepository
{
    Task<Couple> StartNewCouple(Couple customer);
    Task<Couple?> SearchCoupleRelationship(string userOneId, string userIdTwo);
    // Task UpdateCouple(string userId);
}