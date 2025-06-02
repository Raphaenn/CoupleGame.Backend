namespace Application.Dtos;

public class QuizDto
{
    public string QuizId { get; set; }
    public string CoupleId { get; set; }
    public string QuestionId1 { get; set; }
    public string? QuestionId2 { get; set; }
    public string? QuestionId3 { get; set; }
    public string? QuestionId4 { get; set; }
    public string? QuestionId5 { get; set; }
    public string? QuestionId6 { get; set; }
    public DateTime CreatedAt { get; set; }
}