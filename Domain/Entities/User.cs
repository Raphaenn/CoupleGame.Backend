namespace Domain.Entities;
public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public DateTime BirthDate { get; private set; }
    
    // Construtor sem parâmetros para uso exclusivo do ORM.
    // Ele é necessário para que o ORM possa materializar a entidade a partir do banco de dados.
    private User() {}

    public User(Guid id, string name, string email, string passsword, DateTime birthdate)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Name cannot be empty");
        if (string.IsNullOrEmpty(email))
            throw new ArgumentException("Email cannot be empty");
        if (birthdate >= DateTime.Now)
            throw new ArgumentException("Date of birth must be in the past");

        Id = id;
        Name = name;
        Email = ValidateEmail(email);
        Password = passsword;
        BirthDate = birthdate;
    }
    
    private string ValidateEmail(string email)
    {
        return this.Email = email;
    }
}