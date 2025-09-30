namespace Domain.ValueObjects;

public readonly record struct EloParams(int KFactor, int Scale = 400, bool AllowDraws = false)
{
    public static EloParams Default => new(32, 400, false);
}