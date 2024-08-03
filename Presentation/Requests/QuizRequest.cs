namespace Api.Requests;

public class QuizRequest
{
    public string QuizId { get; set; } = String.Empty;
    public string QuestionId { get; set; } = String.Empty;
}

public class StartQuizRequest
{
    public string CoupleId { get; set; } = String.Empty;
    public string QuestionId { get; set; } = String.Empty;
}