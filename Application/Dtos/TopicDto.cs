using Domain.Entities;

namespace Application.Dtos;

public class TopicDto
{
    public string Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool Status { get; set; }
    public Question Questions { get; set; }
}