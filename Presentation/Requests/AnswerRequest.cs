namespace Api.Requests;

public class InitAnswerRequest
{
    public string UserId { get; set; } = String.Empty;
    public string QuizId { get; set; } = String.Empty;
    public string Answer { get; set; } = String.Empty;
}

public class UpdateAnswerRequest {
    public string Id { get; set; } = String.Empty;
    public string Answer { get; set; } = String.Empty;
}