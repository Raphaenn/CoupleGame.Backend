namespace Domain.Entities;

public class PceAnswer
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid QuizId { get; set; }
    public Guid QuestionId { get; set; }
    public string Content { get; set; } = String.Empty;
    public DateTime CreatedAt { get; set; }

    private PceAnswer(Guid id, Guid userId, Guid quizId, Guid questionId, string content, DateTime createdAt)
    {
        Id = id;
        UserId = userId;
        QuizId = quizId;
        QuestionId = questionId;
        Content = content;
        CreatedAt = createdAt;
    }

    public static PceAnswer StartAnswer(Guid userId, Guid quizId, Guid questionId, string content, DateTime createdAt)
    {
        return new PceAnswer(Guid.NewGuid(), userId, quizId, questionId, content, createdAt);
    }

    public static PceAnswer Rehydrate(Guid id, Guid userId, Guid quizId, Guid questionId, string content, DateTime createdAt)
    {
        return new PceAnswer(id, userId, quizId, questionId, content, createdAt);
    }
}