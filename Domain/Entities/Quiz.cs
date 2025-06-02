namespace Domain.Entities;

public class Quiz
{
    public Guid Id { get; private set; }
    public Guid CoupleId { get; private set; }
    public Guid Question1 { get; private set; }
    public Guid? Question2 { get; private set; }
    public Guid? Question3 { get; private set; }
    public Guid? Question4 { get; private set; }
    public Guid? Question5 { get; private set; }
    public Guid? Question6 { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Quiz(Guid id, Guid coupleId, Guid question1, Guid? question2, Guid? question3, Guid? question4, Guid? question5, Guid? question6, DateTime createdAt)
    {
        Id = id;
        CoupleId = coupleId;
        Question1 = question1;
        Question2 = question2;
        Question3 = question3;
        Question4 = question4;
        Question5 = question5;
        Question6 = question6;
        CreatedAt = createdAt;
    }

    public static Quiz StartQuiz(Guid coupleId, Guid question1)
    {
        return new Quiz(Guid.NewGuid(), coupleId, question1, null, null, null, null, null, DateTime.Now);
    }

    public static Quiz Rehydrate(Guid id, Guid coupleId, Guid question1, Guid? question2, Guid? question3, Guid? question4, Guid? question5, Guid? question6, DateTime createdAt)
    {
        return new Quiz(Guid.NewGuid(), coupleId, question1, question2, question3, question4, question5, question6, DateTime.Now);
    }
}