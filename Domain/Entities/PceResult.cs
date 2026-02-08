namespace Domain.Entities;

public class PceResult
{
    // id: 1,
    // quizId: 1,
    // topicId,
    // questionContent, 
    // questionId, 
    // answers: [{ answeredBy: 'A', answer: firstAnswer.answerText, userId: firstAnswer.userId }, { answeredBy: 'B',      answer: answerText, userId: userData?.id }],
    public Guid Id { get; set; }
    public Guid PceId { get; set; }
    public Guid TopicId { get; set; }
    public string TopicName { get; set; } = String.Empty;

    private readonly List<PceAnswer> _pceAnswersList = new List<PceAnswer>();
    public IReadOnlyList<PceAnswer> PceAnswers => _pceAnswersList.AsReadOnly();
    
    // public string QuestionContent { get; set; }

    public void AddPceAnswers(PceAnswer answer)
    {
        // todo: add validations
        _pceAnswersList.Add(answer);
    }
}   