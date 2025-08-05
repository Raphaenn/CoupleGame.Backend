namespace Application.Dtos;

public class AnswerDto
{
    public string Id { get; set; } = String.Empty;
    public string UserId { get; set; } = String.Empty;
    public string QuizId { get; set; } = String.Empty;
    public string Answer1 { get; set; } = String.Empty;
    public string? Answer2 { get; set; } = String.Empty;
    public string? Answer3 { get; set; } = String.Empty;
    public string? Answer4 { get; set; } = String.Empty;
    public string? Answer5 { get; set; } = String.Empty;
    public string? Answer6 { get; set; } = String.Empty;
    public DateTime CreatedAt { get; set; }
}