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
    
    public AnswerDto ToDto()
    {
        return new AnswerDto
        {
            Id = this.Id,
            UserId = this.UserId,
            QuizId = this.QuizId,
            Answer1 = this.Answer1,
            Answer2 = this.Answer2,
            Answer3 = this.Answer3,
            Answer4 = this.Answer4,
            Answer5 = this.Answer5,
            Answer6 = this.Answer6,
            CreatedAt = this.CreatedAt
        };
    }
}