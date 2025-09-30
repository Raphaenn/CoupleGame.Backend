using Domain.ValueObjects;

namespace Domain.Entities;

public class Ladder
{
    public LadderId Id { get; set; }
    public string Name { get; set; }
    public EloParams Elo { get; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<PersonRating> PersonRatings { get; set; } = new List<PersonRating>();
    public ICollection<MatchVote> MatchVotes { get; set; } = new List<MatchVote>();
    
    
    public Ladder(LadderId id, string name, EloParams elo)
    {
        Id = id;
        Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("name") : name;
        Elo = elo;
    }
}