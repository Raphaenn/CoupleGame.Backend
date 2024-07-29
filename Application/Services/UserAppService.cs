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

    public async Task<UserDto> CreateUser(UserDto user)
    {
        try
        {
            Guid userId = Guid.NewGuid();
            User createdUser = new User(id: userId, name: user.Name, email: user.Email, passsword: user.Password, birthdate: user.Birthdate);

            await _userRepository.CreateUser(createdUser);
            
            user.Id = userId.ToString();
            return user;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}