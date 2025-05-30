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

    public async Task<CoupleDto> StartCouple(string userId, string type, string status)
    {
        try
        {
            Guid parsedUserId = Guid.Parse(userId);

            if (!Enum.TryParse<CoupleTypes>(type, out var parsedType))
            {
                throw new ArgumentException("Invalid couple type");
            }
            
            if (!Enum.TryParse<CoupleStatus>(status, out var parsedStatus))
            {
                throw new ArgumentException("Invalid couple status");
            }

            Couple coupleInstance = Couple.CreateCouple(parsedUserId, parsedType, parsedStatus);

            await _coupleRepository.StartNewCouple(coupleInstance);
            CoupleDto response = new CoupleDto
            {
                UserOneId = coupleInstance.CoupleOne.ToString(),
                UserTwoId = null,
                Status = coupleInstance.Status.ToString(),
                Type = coupleInstance.Type.ToString()
            };

            return response;
        }
        catch (Exception e)
        {
            throw new Exception(message: e.Message);
        }
    }

    public async Task<CoupleDto> AddSecondMember(string coupleId, string userId)
    {
        return null;
    }
}