using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Services;

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
                Id = coupleInstance.Id.ToString(),
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

    public async Task AddSecondMember(string coupleId, string userId)
    {
        try
        {
            Guid parsedCoupleId = Guid.Parse(coupleId);
            Guid parsedUserId = Guid.Parse(userId);
            Couple couple = await _coupleRepository.SearchCoupleById(parsedCoupleId);
            if (couple == null)
            {
                throw new Exception("Wrong couple id");
            }
            couple.AddMember(parsedUserId);
            await _coupleRepository.AddCoupleMember(parsedCoupleId, parsedUserId);
            return;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<CoupleDto> GetCouplePartner(Guid userId)
    {
        Couple? couple = await _coupleRepository.GetLongTermCouple(userId);
        if (couple == null)
        {
            throw new ArgumentException("Wrong couple id");
        }

        CoupleDto parser = new CoupleDto
        {
            Id = couple.Id.ToString(),
            UserOneId = couple.CoupleOne.ToString(),
            UserTwoId = couple.CoupleTwo.ToString(),
            Type = couple.Type.ToString(),
            Status = couple.Status.ToString()
        };
        
        return parser;
    }
    
    public async Task<CoupleDto> CreateTempCouple(string userId, CancellationToken ct)
    {
        Guid parsedUserId1 = Guid.Parse(userId);

        Couple coupleInstance = Couple.CreateCouple(parsedUserId1, CoupleTypes.Temporary, CoupleStatus.Active);
        coupleInstance.AddMember(Guid.Parse("d2e275a4-dbb1-4e3b-909f-6a330a166685"));

        await _coupleRepository.StartNewCouple(coupleInstance);
        CoupleDto response = new CoupleDto
        {
            Id = coupleInstance.Id.ToString(),
            UserOneId = coupleInstance.CoupleOne.ToString(),
            UserTwoId = coupleInstance.CoupleTwo.ToString(),
            Status = coupleInstance.Status.ToString(),
            Type = coupleInstance.Type.ToString()
        };

        return response;
    }
    
    public async Task<CoupleDto> GetTempCouple(string userId, string userId2)
    {
        Guid parsedId1 = Guid.Parse(userId);
        Guid parsedId2 = Guid.Parse(userId2);
        Couple? couple = await _coupleRepository.SearchTemCouple(parsedId1, parsedId2);
        
        if (couple == null)
        {
            throw new ArgumentException("Wrong couple id");
        }

        CoupleDto parser = new CoupleDto
        {
            Id = couple.Id.ToString(),
            UserOneId = couple.CoupleOne.ToString(),
            UserTwoId = couple.CoupleTwo.ToString(),
            Type = couple.Type.ToString(),
            Status = couple.Status.ToString()
        };
        
        return parser;
    }
}