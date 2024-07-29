namespace Application.Dtos;

public class QuestionDto
{
    public string? Id { get; set; }
    public string TopicId { get; set; } = String.Empty;
    public string QuestionText { get; set; } = String.Empty;
    public string Answer1 { get; set; } = String.Empty;
    public string Answer2 { get; set; } = String.Empty;
    public string Answer3 { get; set; } = String.Empty;
    public string Answer4 { get; set; } = String.Empty;
}