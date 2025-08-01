namespace Domain.Entities;

public class UserPreferences
{
    public Guid Id { get; set; } 
    public Guid UserId { get; private set; }
    public string Location { get; private set; }
    public decimal? DistanceKm { get; private set; }
    public string GenderPreference { get; private set; }
    public int? AgeMin { get; private set; }
    public int? AgeMax { get; private set; }
    public decimal? HeightMin { get; private set; }
    public decimal? HeightMax { get; private set; }
    public decimal? WeightMin { get; private set; }
    public decimal? WeightMax { get; private set; }
    public List<string> Interests { get; private set; } = new();
    
    public UserPreferences(Guid userId, string location, decimal? distanceKm, string genderPreference,int? ageMin, int? ageMax,decimal? heightMin, decimal? heightMax,decimal? weightMin, decimal? weightMax,List<string> interests)
    {
        UserId = userId;
        Location = location;
        DistanceKm = distanceKm;
        GenderPreference = genderPreference;
        AgeMin = ageMin;
        AgeMax = ageMax;
        HeightMin = heightMin;
        HeightMax = heightMax;
        WeightMin = weightMin;
        WeightMax = weightMax;
        Interests = interests ?? new List<string>();
    }

    public void UpdatePreferences(string location, decimal? distanceKm, string genderPreference, int? ageMin, int? ageMax, decimal? heightMin, decimal? heightMax, decimal? weightMin, decimal? weightMax, List<string> interests)
    {
        Location = location;
        DistanceKm = distanceKm;
        GenderPreference = genderPreference;
        AgeMin = ageMin;
        AgeMax = ageMax;
        HeightMin = heightMin;
        HeightMax = heightMax;
        WeightMin = weightMin;
        WeightMax = weightMax;
        Interests = interests ?? new List<string>();
    }
}