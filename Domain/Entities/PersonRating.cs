using Domain.ValueObjects;

namespace Domain.Entities;

public class PersonRating
{
    public Guid UserId { get; set; }
    public LadderId LadderId { get; set; }

    public double Rating { get; set; } = 1500;
    public int Wins { get; set; }
    public int Losses { get; set; }
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    // Concurrency control (optimistic): incrementado a cada update
    public int Version { get; set; }

    public Ladder Ladder { get; set; } = null!;
    public User User { get; set; } = null!; // (adicione se tiver a entidade User)
    
    public PersonRating(LadderId ladderId, Guid userId, double rating = 1500, int wins = 0, int losses = 0, int version = 0)
    {
        LadderId = ladderId;
        UserId = userId;
        Rating = rating;
        Wins = wins;
        Losses = losses;
        Version = version;
    }

    public void ApplyWin(double newRating)
    {
        Wins++;
        Rating = newRating;
        Version++;
    }

    public void ApplyLoss(double newRating)
    {
        Losses++;
        Rating = newRating;
        Version++;
    }
}