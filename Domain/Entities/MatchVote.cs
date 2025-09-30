using Domain.ValueObjects;

namespace Domain.Entities;

public class MatchVote
{
    public Guid Id { get; set; }
    public LadderId LadderId { get; set; }
    public Guid UserAId { get; set; }
    public Guid UserBId { get; set; }
    public Guid WinnerGuid { get; set; }
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
    
    public MatchVote(
        Guid id, LadderId ladderId, Guid a, Guid b, Guid winner, DateTimeOffset occurredAt,
        int kUsed, double expectedWinner, double expectedLoser,
        double winnerBefore, double winnerAfter, double loserBefore, double loserAfter,
        string? idempotencyKey = null, Guid? sessionUser = null, string? clientIp = null)
    {
        if (a == b) throw new ArgumentException("A e B devem ser distintos.");
        if (winner != a && winner != b) throw new ArgumentException("Winner deve ser A ou B.");

        Id = id;
        LadderId = ladderId;
        UserAId = a;
        UserBId = b;
        WinnerGuid = winner;
        CreatedAt = occurredAt;

        KUsed = kUsed;
        ExpectedWinner = expectedWinner;
        ExpectedLoser  = expectedLoser;
        RatingWinnerBefore = winnerBefore;
        RatingWinnerAfter  = winnerAfter;
        RatingLoserBefore  = loserBefore;
        RatingLoserAfter   = loserAfter;
        DeltaWinner  = winnerAfter - winnerBefore;
        DeltaLoser   = loserAfter  - loserBefore;

        IdempotencyKey = idempotencyKey;
        SessionId = sessionUser;
        ClientIp = clientIp;
    }
}