using Domain.Entities;

namespace Application.Dtos;

public class PceDto
{
    public Guid Id { get; set; }
    public Guid CoupleId { get; set; }
    public PceStatus Status { get; set; } = PceStatus.Pending;
    public DateTime CreatedAt { get; set; }
}

public class PceResultDto
{
    public Guid Id { get; set; }
    public Guid PceId { get; set; }
    public Guid TopicId { get; set; }
    public string TopicName { get; set; } = "";

    public List<PceQuestionAnswersDto> Questions { get; set; } = new();
}

public class PceQuestionAnswersDto
{
    public Guid QuestionId { get; set; }
    public string QuestionText { get; set; } = String.Empty;
    public List<PceAnswerDto> Answers { get; set; } = new();
}

public class PceAnswerDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid PceId { get; set; }
    public Guid QuestionId { get; set; }
    public Guid TopicId { get; set; }
    public string Content { get; set; } = String.Empty;
    public DateTime CreatedAt { get; set; }
}