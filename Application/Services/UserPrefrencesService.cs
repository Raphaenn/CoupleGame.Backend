using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class UserPrefrencesService : IUserPrefrencesService
{
    private readonly IUserPreferencesRepository _userPreferencesRepository;
    
    public UserPrefrencesService(IUserPreferencesRepository userPreferencesRepository)
    {
        _userPreferencesRepository = userPreferencesRepository;
    }
    
    public async Task<UserPreferencesDto> CreateUserPreferences(UserPreferencesDto user)
    {
        try
        {
            UserPreferences createdUser = new UserPreferences(userId: user.UserId, location: user.Location, distanceKm:user.DistanceKm, genderPreference: user.GenderPreference, ageMin: user.AgeMin, ageMax: user.AgeMax, heightMin: user.HeightMin, heightMax: user.HeightMax, weightMin: user.WeightMin, weightMax: user.WeightMax, interests: user.Interests);

            UserPreferences persistency = await _userPreferencesRepository.Create(createdUser);
            user.Id = persistency.UserId;
            return user;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}