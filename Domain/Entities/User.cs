namespace Domain.Entities;
public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = String.Empty;
    public string Email { get; private set; } = String.Empty;
    public string Password { get; private set; } = String.Empty;
    public DateTime BirthDate { get; private set; }
    public double Height  { get; private set; }
    public double Weight   { get; private set; }
    public double Latitude   { get; private set; }
    public double Longitude   { get; private set; }
    
    
    // Construtor sem parâmetros para uso exclusivo do ORM.
    // Ele é necessário para que o ORM possa materializar a entidade a partir do banco de dados.
    private User() {}

    public User(Guid id, string name, string email, string passsword, DateTime birthdate, double height, double weight)
    {
        Id = string.IsNullOrEmpty(name) ? throw new ArgumentException("Name cannot be empty"): id;
        Name = string.IsNullOrEmpty(email) ? throw new ArgumentException("Email cannot be empty") : name;
        Email = ValidateEmail(email);
        Password = passsword;
        BirthDate = birthdate >= DateTime.Now ? throw new ArgumentException("Date of birth must be in the past") : birthdate;
        Height = height;
        Weight = weight;
    }
    
    private string ValidateEmail(string email)
    {
        return this.Email = email;
    }

    public void RegisterLocalidad(double latitude, double longitude)
    {
        Latitude = latitude != double.MinValue ? latitude : throw new ArgumentException("Latitude cannot be negative");
        Longitude = longitude != double.MinValue ? longitude : throw new ArgumentException("Longitude cannot be negative");
    }
}