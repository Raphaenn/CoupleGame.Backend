using System.Collections.ObjectModel;

namespace Domain.Entities;

public enum PceStatus
{
    Pending = 0,
    Active = 1,
    Completed = 2
}

// Partner Compatibility Evaluation
public class Pce
{
    public Guid Id { get; private set; }
    public Guid CoupleId { get; private set; }
    public PceStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private readonly List<Question> _pceQuestionsList = new List<Question>();
    public ReadOnlyCollection<Question> PceQuestions => _pceQuestionsList.AsReadOnly();

    private readonly List<PceAnswer> _pceAnswers = new List<PceAnswer>();
    public ReadOnlyCollection<PceAnswer> PceAnswers => _pceAnswers.AsReadOnly();
    
    private Pce(Guid id, Guid coupleId, PceStatus status, DateTime createdAt)
    {
        Id = id;
        CoupleId = coupleId;
        Status = status;
        CreatedAt = createdAt;
    }

    public static Pce StartPce(Guid coupleId)
    {
        return new Pce(Guid.NewGuid(), coupleId, PceStatus.Pending, DateTime.Now);
    }

    public static Pce Rehydrate(Guid id, Guid coupleId, PceStatus status, DateTime createdAt)
    {
        return new Pce(id, coupleId, PceStatus.Pending, DateTime.Now);
    }

    public void AddPceQuestion(Question question)
    {
        if (question == null)
            throw new ArgumentNullException(nameof(question));

        _pceQuestionsList.Add(question);
    }

    public void ChangeStatus(PceStatus status)
    {
        Status = status;
    }

}