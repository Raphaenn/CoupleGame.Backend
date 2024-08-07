namespace Presentation.Requests;

public class QuizRequest
{
    public string QuizId { get; set; }
    public string QuestionId { get; set; }  
}

public class StartQuizRequest
{
    public string CoupleId { get; set; }    
    public string QuestionId { get; set; }  
}