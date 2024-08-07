namespace Presentation.Requests;

public class InitAnswerRequest
{
    public string UserId { get; set; }
    public string QuizId { get; set; }
    public string Answer { get; set; }
}

public class UpdateAnswerRequest {
    public string Id { get; set; }
    public string Answer { get; set; }
}