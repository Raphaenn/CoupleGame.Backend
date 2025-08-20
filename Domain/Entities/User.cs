namespace Domain.Entities;
public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = String.Empty;
    public string Email { get; private set; } = String.Empty;
    
    public User(Guid id, string name, string email)
    {
        Id = string.IsNullOrEmpty(name) ? throw new ArgumentException("Name cannot be empty"): id;
        Name = string.IsNullOrEmpty(email) ? throw new ArgumentException("Email cannot be empty") : name;
        Email = ValidateEmail(email);
    }
    
    public static User Rehydrate(Guid id, string name, string email)
    {
        return new User(id, name, email);
    }
    
    private string ValidateEmail(string email)
    {
        return this.Email = email;
    }
}