namespace Domain.Entities;

public class PceAnswer
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid PceId { get; set; }
    public string? QuestionText { get; set; } = String.Empty;
    public Guid QuestionId { get; set; }
    public Guid TopicId { get; set; }
    public string Content { get; set; } = String.Empty;
    public DateTime CreatedAt { get; set; }

    private PceAnswer(Guid id, Guid userId, Guid pceId, Guid questionId, Guid topicId, string content, DateTime createdAt)
    {
        Id = id;
        UserId = userId;
        PceId = pceId;
        QuestionId = questionId;
        TopicId = topicId;
        Content = content;
        CreatedAt = createdAt;
    }

    public static PceAnswer StartAnswer(Guid userId, Guid pceId, Guid questionId, Guid topicId, string content)
    {
        return new PceAnswer(Guid.NewGuid(), userId, pceId, questionId, topicId, content, DateTime.Now);
    }

    public static PceAnswer Rehydrate(Guid id, Guid userId, Guid pceId, Guid questionId, Guid topicId, string content, DateTime createdAt)
    {
        return new PceAnswer(id, userId, pceId, questionId, topicId, content, createdAt);
    }

    public void AddQuestionText(string text)
    {
        QuestionText = text;
    }
}