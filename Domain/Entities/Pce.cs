using System.Collections.ObjectModel;

namespace Domain.Entities;

public enum PceStatus
{
    Active,
    Pending,
    Completed
}

// Partner Compatibility Evaluation
public class Pce
{
    public Guid Id { get; private set; }
    public Guid CoupleId { get; private set; }
    public PceStatus Status { get; private set; } = PceStatus.Pending;
    public DateTime CreatedAt { get; private set; }

    private readonly List<Question> _premiumQuestionsList = new List<Question>();
    public ReadOnlyCollection<Question> PremiumQuestions => _premiumQuestionsList.AsReadOnly();

    private readonly List<PceAnswer> _premiumAnswers = new List<PceAnswer>();
    public ReadOnlyCollection<PceAnswer> PremiumAnswers => _premiumAnswers.AsReadOnly();
    
    private Pce(Guid id, Guid coupleId, PceStatus status, DateTime createdAt)
    {
        Id = id;
        CoupleId = coupleId;
        Status = status;
        CreatedAt = createdAt;
    }

    public static Pce StartPremiumQuiz(Guid coupleId, Guid question1)
    {
        return new Pce(Guid.NewGuid(), coupleId, PceStatus.Pending, DateTime.Now);
    }

    public static Pce Rehydrate(Guid id, Guid coupleId, PceStatus status, DateTime createdAt)
    {
        return new Pce(id, coupleId, PceStatus.Pending, DateTime.Now);
    }

    public void AddPremiumQuestion(Question question)
    {
        if (question == null)
            throw new ArgumentNullException(nameof(question));

        _premiumQuestionsList.Add(question);
    }

}