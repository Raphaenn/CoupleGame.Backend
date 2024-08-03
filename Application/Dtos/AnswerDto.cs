namespace Application.Dtos;

public class AnswerDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Answer1 { get; set; } = String.Empty;
    public string? Answer2 { get; set; }
    public string? Answer3 { get; set; }
    public string? Answer4 { get; set; }
    public string? Answer5 { get; set; }
    public string? Answer6 { get; set; }
    public DateTime CreatedAt { get; set; }
}