using Application.Dtos;

namespace Application.Interfaces;

public interface ICoupleAppService
{
    Task<CoupleDto> StartCouple(string userId, string type, string status);

    Task<CoupleDto> AddSecondMember(string coupleId, string userId);
}