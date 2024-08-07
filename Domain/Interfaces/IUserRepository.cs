using Domain.Entities;

namespace Domain.Interfaces;
public interface IUserRepository
{
    Task<User> CreateUser(User customer);
}