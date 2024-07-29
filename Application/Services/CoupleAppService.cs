using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class CoupleAppService : ICoupleAppService
{
    private readonly ICoupleRepository _coupleRepository;

    public CoupleAppService(ICoupleRepository coupleRepository)
    {
        this._coupleRepository = coupleRepository;
    }

    public async Task<CoupleDto> CreateRelationship(string userOne, string userTwo)
    {
        try
        {
            Couple? checkRelationship = await _coupleRepository.SearchCoupleRelationship(userOne, userTwo);
            if (checkRelationship != null)
            {
                throw new Exception(message: "Relationship already exists");
            }
            
            Guid relationshipId = Guid.NewGuid();
            Guid userIdOne = Guid.Parse(userOne);
            Guid userIdTwo = Guid.Parse(userTwo);
            DateTime createdAt = DateTime.Now;

            Couple coupleInstance = new Couple(id: relationshipId, coupleOne: userIdOne, coupleTwo: userIdTwo, type: "friendly", status: "active", createdAt: createdAt);

            await _coupleRepository.CreateCouple(coupleInstance);
            CoupleDto response = new CoupleDto
            {
                UserOneId = userOne,
                UserTwoId = userTwo,
                CreatedAt = createdAt
            };

            return response;
        }
        catch (Exception e)
        {
            throw new Exception(message: e.Message);
        }
    }
}