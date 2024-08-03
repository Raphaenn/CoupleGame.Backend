namespace Application.Dtos;

public class QuizDto
{
    public string CoupleId { get; set; } = String.Empty;
    public string QuestionId1 { get; set; } = String.Empty;
    public string? QuestionId2 { get; set; }
    public string? QuestionId3 { get; set; }
    public string? QuestionId4 { get; set; }
    public string? QuestionId5 { get; set; }
    public string? QuestionId6 { get; set; }
    public DateTime CreatedAt { get; set; }
}