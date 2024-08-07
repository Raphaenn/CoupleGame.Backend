using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Services;

public class UserService : IUserRepository
{
    private readonly IUserRepository _userRepository;

    public UserService (IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<User> CreateUser(User user)
    {
        return await _userRepository.CreateUser(user);
    }
}