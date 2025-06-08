namespace Domain.Entities;

public class Answers
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid QuizId { get; set; }
    public string Answer1 { get; set; } = String.Empty;
    public string? Answer2 { get; set; }
    public string? Answer3 { get; set; }
    public string? Answer4 { get; set; }
    public string? Answer5 { get; set; }
    public string? Answer6 { get; set; }
    public DateTime CreatedAt { get; set; }

    private Answers(Guid id, Guid userId, Guid quizId, string answer1, string answer2, string answer3, string answer4, string answer5, string answer6, DateTime createdAt)
    {
        Id = id;
        UserId = userId;
        QuizId = quizId;
        Answer1 = answer1;
        Answer2 = answer2;
        Answer3 = answer3;
        Answer4 = answer4;
        Answer5 = answer5;
        Answer6 = answer6;
        CreatedAt = createdAt;
    }

    public static Answers StartAnswer(Guid userId, Guid quizId, string answer1, string answer2, string answer3, string answer4, string answer5, string answer6)
    {
        return new Answers(Guid.NewGuid(), userId, quizId, answer1, answer2, answer3, answer4, answer5, answer6, DateTime.Now);
    }

    public static Answers Rehydrate(Guid id, Guid userId, Guid quizId, string answer1, string answer2, string answer3, string answer4, string answer5, string answer6, DateTime createdAt)
    {
        return new Answers(id, userId, quizId, answer1, answer2, answer3, answer4, answer5, answer6, createdAt);
    }

    public bool UpdateAnswer(string answer)
    {
        if (Answer2 == null) { Answer2 = answer; return true; }
        if (Answer3 == null) { Answer3 = answer; return true; }
        if (Answer4 == null) { Answer4 = answer; return true; }
        if (Answer5 == null) { Answer5 = answer; return true; }
        if (Answer6 == null) { Answer6 = answer; return true; }
        return false;
    }
}