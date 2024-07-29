namespace Application.Dtos;
public class CoupleDto
{
    public string UserOneId { get; set; } = String.Empty;
    public string UserTwoId { get; set; } = String.Empty;
    public DateTime CreatedAt { get; set; }
}