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
    public Guid QuizId { get; set; }
    public Guid TopicId { get; set; }
    public Guid QuestionId { get; set; }

    private readonly List<PceAnswer> _premiumAnswersList = new List<PceAnswer>();
    public IReadOnlyList<PceAnswer> PremiumAnswers => _premiumAnswersList.AsReadOnly();
    
    // public string QuestionContent { get; set; }
}