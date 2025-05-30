namespace Application.Dtos;
public class CoupleDto
{
    public string UserOneId { get; set; } = String.Empty;
    public string? UserTwoId { get; set; } = String.Empty;
    public string? Type { get; set; }
    public string? Status { get; set; }
}