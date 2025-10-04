using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Services;

public enum InteractionType
{
    Like,        // 0.6
    Match,       // 1.0
    Dislike,     // 0.0
    Skip         // 0.3
}

public sealed class EloRatingResult
{
    // Pure formula
    private static double Expected(double rA, double rB, int scale)
        => 1.0 / (1.0 + Math.Pow(10.0, (rB - rA) / (double)scale));
    
    private static (double newA, double newB, double eA, double eB) Update(double rA, double rB, int k, double scoreA, int scale)
    {
        var eA = Expected(rA, rB, scale); // probabilidade esperada de A vencer.
        var eB = 1.0 - eA;                // probabilidade de B (complemento).
        var newA = rA + k * (scoreA - eA);            // novo rating de A.
        var newB = rB + k * ((1.0 - scoreA) - eB);    // novo rating de B.
        return (newA, newB, eA, eB);      // retorna tudo como tupla nomeada.
    }
    
    // método público que aplica o resultado ao par (A,B) e devolve dados úteis (antes/depois etc.).
    public EloResult Apply(PersonRating a, PersonRating b, InteractionType interaction, EloParams p)
    {
        if (ReferenceEquals(a, b) || a.UserId == b.UserId)
            throw new Exception("A e B must be different.");

        // guarda os ratings antes do ajuste (útil para logs/histórico e para calcular deltas).
        var aBefore = a.Rating;
        var bBefore = b.Rating;

        var scoreA = interaction switch
        {
            InteractionType.Match   => 1.0, // Vitória completa
            InteractionType.Like    => 0.6, // Vitória parcial
            InteractionType.Skip    => 0.3, // Derrota leve / neutra
            InteractionType.Dislike => 0.0, // Derrota total
            _ => throw new ArgumentOutOfRangeException()
        };

        // calcula expectativa (eA/eB) e novos ratings (newA/newB) usando K e scale da ladder.
        var (newA, newB, eA, eB) = Update(aBefore, bBefore, p.KFactor, scoreA, p.Scale);

        // aplica nas entidades
        if (scoreA > eA)
        {
            a.ApplyWin(newA);
            b.ApplyLoss(newB);
        }
        else
        {
            a.ApplyLoss(newA);
            b.ApplyWin(newB);
        }

        // monta e retorna um DTO imutável com expectativas, ratings antes/depois
        // (serve para persistir em match_vote/rating_history e para telemetria).
        return new EloResult(
            ExpectedA: eA,
            ExpectedB: eB,
            ABefore: aBefore, AAfter: newA,
            BBefore: bBefore, BAfter: newB
        );
    }
}

public sealed record EloResult(
    double ExpectedA, double ExpectedB,
    double ABefore,  double AAfter,
    double BBefore,  double BAfter)
{
    public double DeltaA => AAfter - ABefore;
    public double DeltaB => BAfter - BBefore;
}