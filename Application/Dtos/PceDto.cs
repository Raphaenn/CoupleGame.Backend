using Domain.Entities;

namespace Application.Dtos;

public class PceDto
{
    public Guid Id { get; set; }
    public Guid CoupleId { get; set; }
    public string Status { get; set; } = String.Empty;
    public DateTime CreatedAt { get; set; }
}

public class PceResultDto
{
    public Guid Id { get; set; }
    public Guid PceId { get; set; }
    public Guid TopicId { get; set; }
    public string TopicName { get; set; } = String.Empty;

    public IReadOnlyList<PceAnswer> PceAnswersList { get; set; }
}