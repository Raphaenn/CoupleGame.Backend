namespace Domain.Entities;

public class Quiz
{
    public Guid Id { get; set; }
    public Guid CoupleId { get; set; }
    public Guid QuestionId1 { get; set; }
    public Guid QuestionId2 { get; set; }
    public Guid QuestionId3 { get; set; }
    public Guid QuestionId4 { get; set; }
    public Guid QuestionId5 { get; set; }
    public Guid QuestionId6 { get; set; }
    public DateTime CreatedAt { get; set; }
}