namespace Application.Dtos;

public class UserDto
{
    public string? Id { get; set; } = String.Empty;
    public string Name { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
    public DateTime Birthdate { get; set; }
    public double Height  { get; set; }
    public double Weight   { get; set; }
}