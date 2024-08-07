namespace Application.Dtos;

public class UserDto
{
    public string? Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime Birthdate { get; set; }
}