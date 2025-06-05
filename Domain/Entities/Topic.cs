namespace Domain.Entities;

public class Topic
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool Status { get; private set; }

    private readonly List<Question> _questionList = new();
    public IReadOnlyCollection<Question> Questions => _questionList.AsReadOnly();

    public Topic(Guid id, string name, string description, bool status)
    {
        this.Id = id;
        this.Name = SetName(name);
        Description = SetDescription(description);
        this.Status = status;
    }

    private static string SetName(string name)
    {
        if (String.IsNullOrEmpty(name))
        {
            throw new Exception(message: "Topic name cannot be empty or null");
        }

        return name;
    }
    
    public void AddQuestion(Question questions)
    {
        _questionList.Add(questions);
    }

    public Question GetRandomQuestion()
    {
        if (!_questionList.Any())
            throw new InvalidOperationException("This topic has no questions");

        var random = new Random();
        return _questionList[random.Next(_questionList.Count)];
    }
    
    private static string SetDescription(string description)
    {
        if (string.IsNullOrEmpty(description))
        {
            throw new ArgumentException("Description cannot be empty or null");
        }

        return description;
    }
}