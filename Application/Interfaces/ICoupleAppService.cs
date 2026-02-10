using Application.Dtos;

namespace Application.Interfaces;

public interface ICoupleAppService
{
    Task<CoupleDto> StartCouple(string userId, string type, string status);
    
    Task<CoupleDto> CreateTempCouple(string userId, CancellationToken ct);
    
    Task<CoupleDto> GetTempCouple(string userId, string userId2);

    Task AddSecondMember(string coupleId, string userId);

    Task<CoupleDto> GetCouplePartner(Guid userId);
}