namespace Domain.Entities;

public class Question
{
    public string Id { get; private set; }
    public string TopicId { get; private set; }
    public string QuestionText { get; private set; }
    public string Answer1 { get; private set; }
    public string Answer2 { get; private set; }
    public string Answer3 { get; private set; }
    public string Answer4 { get; private set; }


    public Question(
        string id,
        string topicId,
        string questionText,
        string answer1,
        string answer2,
        string answer3,
        string answer4
        )
    {
        this.Id = !string.IsNullOrEmpty(id) ? id : throw new ArgumentException("Id cannot be empty");
        this.TopicId = !string.IsNullOrEmpty(topicId) ? topicId : throw new ArgumentException("Id cannot be empty");
        this.QuestionText = questionText;
        this.Answer1 = answer1;
        this.Answer2 = answer2;
        this.Answer3 = answer3;
        this.Answer4 = answer4;
    }
}