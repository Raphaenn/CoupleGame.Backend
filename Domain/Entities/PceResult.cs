namespace Domain.Entities;

public class PceResult
{
    public Guid Id { get; set; }
    public Guid PceId { get; set; }
    public Guid TopicId { get; set; }
    public string TopicName { get; set; } = String.Empty;

    private readonly List<PceAnswer> _pceAnswersList = new List<PceAnswer>();
    public IReadOnlyList<PceAnswer> PceAnswers => _pceAnswersList.AsReadOnly();
    

    public void AddPceAnswers(PceAnswer answer)
    {
        // todo: add validations
        _pceAnswersList.Add(answer);
    }
}   