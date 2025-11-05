using Application.Dtos;

namespace Application.Interfaces;

public interface IInteractionAppService
{
    Task CreateUsersInteraction(string actorId, string targetId, string type);

    Task<IReadOnlyList<InteractionDto>> ListUserInteractions(string userId, string type, string? lastId, int sizePlusOne, CancellationToken ct);
}