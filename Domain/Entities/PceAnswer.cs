namespace Domain.Entities;

public class PceAnswer
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid QuizId { get; set; }
    public Guid QuestionId { get; set; }
    public Guid TopicId { get; set; }
    public string Content { get; set; } = String.Empty;
    public DateTime CreatedAt { get; set; }

    private PceAnswer(Guid id, Guid userId, Guid quizId, Guid questionId, Guid topicId, string content, DateTime createdAt)
    {
        Id = id;
        UserId = userId;
        QuizId = quizId;
        QuestionId = questionId;
        TopicId = topicId;
        Content = content;
        CreatedAt = createdAt;
    }

    public static PceAnswer StartAnswer(Guid userId, Guid quizId, Guid questionId, Guid topicId, string content)
    {
        return new PceAnswer(Guid.NewGuid(), userId, quizId, questionId, topicId, content, DateTime.Now);
    }

    public static PceAnswer Rehydrate(Guid id, Guid userId, Guid quizId, Guid questionId, Guid topicId, string content, DateTime createdAt)
    {
        return new PceAnswer(id, userId, quizId, questionId, topicId, content, createdAt);
    }
}