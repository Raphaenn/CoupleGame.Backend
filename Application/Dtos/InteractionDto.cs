namespace Application.Dtos;

public class InteractionDto
{
    public string Id { get; set; } = String.Empty;
    public string ActorId { get; set; } = String.Empty;
    public string TargetId { get; set; } = String.Empty;
    public string Type { get; set; } = String.Empty;
}