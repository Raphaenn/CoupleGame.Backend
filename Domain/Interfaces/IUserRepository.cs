using Domain.Entities;

namespace Domain.Interfaces;
public interface IUserRepository
{
    Task<User> CreateUser(User customer);
    Task<User> SearchUser(Guid userId);
    Task<IReadOnlyList<User>> GetUsersByParams(string city, string sexuality, string sexualOrientation, double? height, double? weight, int sizePlusOne, CancellationToken ct);

    Task<IReadOnlyList<User>> GetUsersByRanking(string city, string sexuality, string sexualOrientation, int sizePlusOne, decimal? lastScore, Guid? lastId, CancellationToken ct);
}