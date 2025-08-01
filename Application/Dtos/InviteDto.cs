namespace Application.Dtos;

public class InviteDto
{
    public string Id { get; set; }
    public string QuizId { get; set; }
    public string Email { get; set; }
    public bool Accepted { get; set; }
    public DateTime CreatedAt { get; set; }
}