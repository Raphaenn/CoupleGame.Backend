using Application.Dtos;

namespace Application.Interfaces;

public interface IUserPrefrencesService
{
    Task<UserPreferencesDto> CreateUserPreferences(UserPreferencesDto userPreferences);
}