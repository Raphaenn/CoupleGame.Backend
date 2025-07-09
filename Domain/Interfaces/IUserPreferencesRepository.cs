using Domain.Entities;

namespace Domain.Interfaces;

public interface IUserPreferencesRepository
{
    Task<UserPreferences> Create(UserPreferences preferences);
}