using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;
public class UserAppService : IUserAppService
{

    private readonly IUserRepository _userRepository;
    
    public UserAppService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> SearchUserService(string id)
    {
        try
        {
            Guid parsedId = Guid.Parse(id);
            User user = await _userRepository.SearchUser(parsedId);
            UserDto userDto = new UserDto
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Email = user.Email,
            };
            return userDto;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}