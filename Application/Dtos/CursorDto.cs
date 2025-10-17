namespace Application.Dtos;

public sealed record RankingCursor(decimal? LastScore, Guid? LastId, int? Size);
public sealed record CursorPage<T>(IReadOnlyList<T> Items, RankingCursor? Next);