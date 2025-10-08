using Domain.Entities;

namespace Domain.Interfaces;
public interface IUserRepository
{
    Task<User> CreateUser(User customer);
    Task<User> SearchUser(Guid userId);
    Task<List<User>> GetUserListByParams(string city);

    Task<List<User>> GetUsersByRanking(Guid userId, string city, string sexualOrientation, int limit, CancellationToken ct);
}