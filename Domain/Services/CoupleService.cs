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

    public async Task<Couple> StartNewCouple(Couple couple)
    {
        Couple response = await _coupleRepository.StartNewCouple(couple);
        return response;
    }

    public async Task<Couple?> SearchCoupleRelationship(string userIdOne, string userIdTwo)
    {
        Couple? response = await _coupleRepository.SearchCoupleRelationship(userIdOne, userIdTwo);
        if (response == null)
        {
            return null;
        }
        return response;
    }
}