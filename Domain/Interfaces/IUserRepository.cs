using Domain.Entities;

namespace Domain.Interfaces;
public interface IUserRepository
{
    Task<User> CreateUser(User customer);
    Task<User> SearchUser(Guid userId);
    Task<List<User>> GetUserListByParams(string city);

    Task<IEnumerable<User>> GetUsersByRanking(string city, string sexualOrientation, int sizePlusOne, decimal? lastScore, Guid? lastId, CancellationToken ct);
}