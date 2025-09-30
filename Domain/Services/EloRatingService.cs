using Domain.Entities;

namespace Domain.Services;

public class EloRatingService
{
    private readonly List<PersonRating> _person;
    private readonly Random _rng = new();
    public static double K = 32;

    // Constructor that receives a list of persons.
    public EloRatingService(IEnumerable<PersonRating> person) => _person = person.ToList();

    // Simple pair selection: pick two with nearest ratings (within a window), else random
    public (PersonRating A, PersonRating B) PickPair(double maxRatingGap = 200)
    {
        if (_person.Count < 2) throw new InvalidOperationException("Need at least 2 person");
        // Start with a random anchor, then find the closest-by-rating other person
        PersonRating a = _person[_rng.Next(_person.Count)];
        List<PersonRating> candidates = _person.Where(p => p.UserId != a.UserId)
            .OrderBy(p => Math.Abs(p.Rating - a.Rating))
            .ToList();
        PersonRating b = candidates.FirstOrDefault(c => Math.Abs(c.Rating - a.Rating) <= maxRatingGap)
                         ?? candidates.First(); // fallback to closest even if outside gap
        return (a, b);
    }
    
    public static double Expected(double ratingA, double ratingB)
        => 1.0 / (1.0 + Math.Pow(10.0, (ratingB - ratingA) / 400.0));

    // New rating after one match (score = 1 win, 0 loss)
    public static double NewRating(double current, double expected, double score)
        => current + K * (score - expected);
    
    // Record a result: winner beats loser
    public void RecordResult(PersonRating winner, PersonRating loser)
    {
        double expW = Expected(winner.Rating, loser.Rating);
        double expL = Expected(loser.Rating, winner.Rating);

        double newWinner = NewRating(winner.Rating, expW, 1);
        double newLoser  = NewRating(loser.Rating,  expL, 0);

        winner.ApplyWin(newWinner);
        loser.ApplyLoss(newLoser);
    }

    public IEnumerable<PersonRating> GetLeaderboard() => _person.OrderByDescending(p => p.Rating);
}