namespace Domain.Entities;

public class Question
{
    public Guid Id { get; private set; }
    public Guid TopicId { get; private set; }
    public string QuestionText { get; private set; }
    public string Answer1 { get; private set; }
    public string Answer2 { get; private set; }
    public string Answer3 { get; private set; }
    public string Answer4 { get; private set; }


    public Question(
        Guid id,
        Guid topicId,
        string questionText,
        string answer1,
        string answer2,
        string answer3,
        string answer4
        )
    {
        this.Id = id != Guid.Empty ? id : throw new ArgumentException("Id cannot be empty");
        this.TopicId = topicId != Guid.Empty ? topicId : throw new ArgumentException("Id cannot be empty");
        this.QuestionText = questionText;
        this.Answer1 = answer1;
        this.Answer2 = answer2;
        this.Answer3 = answer3;
        this.Answer4 = answer4;
    }
}