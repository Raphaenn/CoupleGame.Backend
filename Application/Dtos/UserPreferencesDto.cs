namespace Application.Dtos;

public class UserPreferencesDto
{ 
    public Guid Id { get; set; } 
    public Guid UserId { get; set; }
    public string Location { get; set; }
    public decimal? DistanceKm { get; set; }
    public string? GenderPreference { get; set; } 
    public int? AgeMin { get; set; }
    public int? AgeMax { get; set; }
    public decimal? HeightMin { get; set; }
    public decimal? HeightMax { get; set; }
    public decimal? WeightMin { get; set; }
    public decimal? WeightMax { get; set; }
    public List<string>? Interests { get; set; }
}