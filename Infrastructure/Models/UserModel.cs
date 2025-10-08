namespace Infrastructure.Models;

public class UserModel
{
    public string? Id { get; set; } = String.Empty;
    public string Name { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public int Limit { get; set; } = 10;
}