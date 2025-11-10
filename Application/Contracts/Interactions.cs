namespace Application.Contracts;

public sealed record InteractionFeedDto(
    Guid Id, Guid ActorId, Guid TargetId, string Type,
    string? TargetName, string? TargetEmail, string? TargetCity,
    DateTimeOffset CreatedAt
    );

public interface IInteractionFeedQueries
{
    Task<IReadOnlyList<InteractionFeedDto>> ListByActorAsync(
        Guid actorId, Guid? lastId, int limitPlusOne, CancellationToken ct);
}
