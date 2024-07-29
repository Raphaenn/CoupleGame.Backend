namespace Domain.Entities;

public class Answers
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Answer1 { get; set; } = String.Empty;
    public string Answer2 { get; set; } = String.Empty;
    public string Answer3 { get; set; } = String.Empty;
    public string Answer4 { get; set; } = String.Empty;
    public string Answer5 { get; set; } = String.Empty;
    public string Answer6 { get; set; } = String.Empty;
    public DateTime CreatedAt { get; set; }
}