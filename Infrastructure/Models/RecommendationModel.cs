namespace Infrastructure.Models;

public class RecommendationModel
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = String.Empty;
    public double Rating { get; set; }
    
}

public sealed class Ladder
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public int KFactor { get; set; } = 32;       // k_factor
    public int Scale { get; set; } = 400;        // scale (divisor da fórmula)
    public bool AllowDraws { get; set; }         // allow_draws
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<PersonRating> PersonRatings { get; set; } = new List<PersonRating>();
    public ICollection<MatchVote> MatchVotes { get; set; } = new List<MatchVote>();
}

// PersonRating.cs  (tabela: person_rating)
public sealed class PersonRating
{
    public Guid UserId { get; set; }
    public Guid LadderId { get; set; }

    public double Rating { get; set; } = 1500;
    public int Wins { get; set; }
    public int Losses { get; set; }
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    // Concurrency control (optimistic): incrementado a cada update
    public int Version { get; set; }

    public Ladder Ladder { get; set; } = null!;
    // public User User { get; set; } = null!; // (adicione se tiver a entidade User)
}

// MatchVote.cs  (tabela: match_vote)
public sealed class MatchVote
{
    public Guid Id { get; set; }
    public Guid LadderId { get; set; }
    public Guid UserAId { get; set; }
    public Guid UserBId { get; set; }
    public Guid WinnerUserId { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    // Diagnóstico/Auditoria (opcionais)
    public int KUsed { get; set; }
    public double? ExpectedWinner { get; set; }
    public double? ExpectedLoser { get; set; }
    public double? RatingWinnerBefore { get; set; }
    public double? RatingWinnerAfter { get; set; }
    public double? RatingLoserBefore { get; set; }
    public double? RatingLoserAfter { get; set; }
    public double? DeltaWinner { get; set; }
    public double? DeltaLoser { get; set; }

    // Idempotência/antifraude (opcionais)
    public string? IdempotencyKey { get; set; }
    public Guid? SessionId { get; set; }
    public string? ClientIp { get; set; }  // mapeado como inet

    public Ladder Ladder { get; set; } = null!;
}

// RatingHistory.cs  (tabela: rating_history)
public sealed class RatingHistory
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid LadderId { get; set; }
    public Guid MatchId { get; set; }

    public double RatingBefore { get; set; }
    public double RatingAfter { get; set; }
    public double Delta { get; set; }      // after - before
    public short Result { get; set; }      // 1=win, 0=draw, -1=loss
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public Ladder Ladder { get; set; } = null!;
    public MatchVote Match { get; set; } = null!;
}
