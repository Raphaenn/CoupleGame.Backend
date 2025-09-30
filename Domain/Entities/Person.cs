namespace Domain.Entities;

public sealed class Person
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; }
    public string Name { get; }
    public double Rating { get; private set; }
    public int Wins { get; private set; }
    public int Losses { get; private set; }

    public Person(Guid userId, string name, double initialRating = 1500)
    {
        UserId = userId;
        Name = name;
        Rating = initialRating;
    }

    public void ApplyResult(bool won, double newRating)
    {
        if (won) Wins++;
        else Losses++;
        Rating = newRating;
    }
    
    public override string ToString()                 // Custom string representation for printing.
        => $"{Name,-12} | {Rating,6:F1} | W:{Wins} L:{Losses}"; // Formats aligned columns (name, rating, W/L).
}

public static class Elo
{
    // K can be tuned: 16–40 are common. You can also vary K by rating or games played.
    public static double K = 32;

    // Expected score for A vs B
    public static double Expected(double ratingA, double ratingB)
        => 1.0 / (1.0 + Math.Pow(10.0, (ratingB - ratingA) / 400.0));

    // New rating after one match (score = 1 win, 0 loss)
    public static double NewRating(double current, double expected, double score)
        => current + K * (score - expected);
}