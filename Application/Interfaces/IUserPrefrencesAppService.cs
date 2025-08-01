using Application.Dtos;

namespace Application.Interfaces;

public interface IUserPreferencesAppService
{
    Task<UserPreferencesDto> CreateUserPreferences(UserPreferencesDto userPreferences);
}