using Domain.ValueObjects;

namespace Domain.Entities;
public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = String.Empty;
    public string Email { get; private set; } = String.Empty;
    public double Height { get; private set; }
    public double Weight { get; private set; }
    public decimal Score { get; private set; }
    public DateTime BirthDate { get; private set; }

    private readonly List<UserPhotos> _photos = new List<UserPhotos>();
    public IReadOnlyCollection<UserPhotos> Photos => _photos.AsReadOnly();

    public User(Guid id, string name, string email, double height, double weight, DateTime birthDate)
    {
        Id = string.IsNullOrEmpty(name) ? throw new ArgumentException("Name cannot be empty"): id;
        Name = string.IsNullOrEmpty(email) ? throw new ArgumentException("Email cannot be empty") : name;
        Email = ValidateEmail(email);
        BirthDate = birthDate;
        Height = height;
        Weight = weight;
    }
    
    public static User Rehydrate(Guid id, string name, string email, double height, double weight, DateTime birthDate)
    {
        return new User(id, name, email, height, weight, birthDate);
    }
    
    private string ValidateEmail(string email)
    {
        return this.Email = email;
    }
    
    public void AddPhoto(UserPhotos photo)
    {
        _photos.Add(photo);
    }

    public void AddScore(decimal currentScore)
    {
        this.Score = currentScore;
    }
}