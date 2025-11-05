namespace Application.Dtos;

public record struct PhotoDto(string Url, bool IsProfile);

public class UserDto
{
    public string? Id { get; set; } = String.Empty;
    public string Name { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public double Height { get; set; }
    public double Weight { get; set; }
    public DateTime BirthDate { get; set; }
    
    public decimal Rating { get; set; } = 1500;
    public List<PhotoDto> Photos { get; set; } = new List<PhotoDto>();
}