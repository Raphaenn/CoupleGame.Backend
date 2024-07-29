using Application.Dtos;

namespace Application.Interfaces;

public interface ICoupleAppService
{
    Task<CoupleDto> CreateRelationship(string userOne, string userTwo);
}