namespace Domain.Entities;

public class Topic
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool Status { get; private set; }

    public Topic(Guid id, string name, string description, bool status)
    {
        this.Id = id != Guid.Empty ? id : throw new ArgumentException("Id cannot be empty");
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
    
    public void UpdateName(string name)
    {
        this.Name = SetName(name);
    }
    
    public void ChangeStatus(bool status)
    {
        this.Status = status;
    }

    public void UpdateDescription(string description)
    {
        this.Description = SetDescription(description);
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