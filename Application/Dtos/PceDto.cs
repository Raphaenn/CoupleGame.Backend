using Domain.Entities;

namespace Application.Dtos;

public class PceDto
{
    public Guid Id { get; set; }
    public Guid CoupleId { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PceResultDto
{
    public Guid Id { get; set; }
    public Guid QuizId { get; set; }
    public Guid TopicId { get; set; }
    public Guid QuestionId { get; set; }

    // private readonly List<PceAnswer> _premiumAnswersList = new List<PceAnswer>();
    // public IReadOnlyList<PceAnswer> PremiumAnswers => _premiumAnswersList.AsReadOnly();    
}