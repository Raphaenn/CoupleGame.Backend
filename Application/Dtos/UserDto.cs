namespace Application.Dtos;

public class UserDto
{
    public string? Id { get; set; } = String.Empty;
    public string Name { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public double Height { get; set; }
    public double Weight { get; set; }
    public DateTime BirthDate { get; set; }
    
    public decimal Rating { get; set; } = 1500;
    public List<string> Photos { get; set; } = new();
}