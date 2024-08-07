namespace Domain.Entities;

public class Answers
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string QuizId { get; set; }
    public string Answer1 { get; set; }
    public string Answer2 { get; set; }
    public string Answer3 { get; set; }
    public string Answer4 { get; set; }
    public string Answer5 { get; set; }
    public string Answer6 { get; set; }
    public DateTime CreatedAt { get; set; }
}