namespace Api.Requests;

public class CoupleRequest
{
    public string UserOneId { get; set; } = String.Empty;
    public string? UserTwoId { get; set; } = String.Empty;
    public string CoupleType { get; set; } = String.Empty;
    public string CoupleStatus { get; set; } = String.Empty;
}