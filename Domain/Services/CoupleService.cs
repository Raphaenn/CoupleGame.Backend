using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Services;

public class CoupleService : ICoupleRepository
{
    private readonly ICoupleRepository _coupleRepository;

    public CoupleService(ICoupleRepository coupleRepository)
    {
        _coupleRepository = coupleRepository;
    }

    public async Task<Couple> CreateCouple(Couple couple)
    {
        return await _coupleRepository.CreateCouple(couple);
    }

    public async Task<Couple?> SearchCoupleRelationship(string userIdOne, string userIdTwo)
    {
        return await _coupleRepository.SearchCoupleRelationship(userIdOne, userIdTwo);
    }
}