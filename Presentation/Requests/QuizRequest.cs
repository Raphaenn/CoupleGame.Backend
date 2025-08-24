using Domain.Entities;

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

public class QuizAnswerRequest
{
    public string UserId { get; set; } = String.Empty;
    public string QuizId { get; set; } = String.Empty;
    public string Answer { get; set; } = String.Empty;
}

public class UpdateQuizRequest
{
    public string QuizId { get; set; } = String.Empty;
    public string Status { get; set; } = String.Empty;
}