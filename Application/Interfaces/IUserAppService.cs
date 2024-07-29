using Application.Dtos;

namespace Application.Interfaces;

public interface IUserAppService
{
    Task<UserDto> CreateUser(UserDto user);
}