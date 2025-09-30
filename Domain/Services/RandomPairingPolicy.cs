using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Services;

public class RandomPairingPolicy : IPairingPolicy
{
    private readonly List<PersonRating> _person;
    private readonly Random _rng = new();
    public static double K = 32;
    
    public RandomPairingPolicy(IEnumerable<PersonRating> person) => _person = person.ToList();
    
    public (PersonRating A, PersonRating B) PickPairAsync(double maxRatingGap = 200)
    {
        // todo - Temporary pick pair
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
}