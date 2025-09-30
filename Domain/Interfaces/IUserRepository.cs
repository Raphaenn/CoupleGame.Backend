using Domain.Entities;

namespace Domain.Interfaces;
public interface IUserRepository
{
    Task<User> CreateUser(User customer);
    Task<User> SearchUser(Guid userId);
    // Task UpdateUser(string userId);
    Task<List<User>> GetUserListByParams(string city);
}