using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class InteractionAppService : IInteractionAppService
{
    private readonly IInteractionsRepository _interactionsRepository;

    public InteractionAppService(IInteractionsRepository interactionsRepository)
    {
        _interactionsRepository = interactionsRepository;
    }

    public async Task CreateUsersInteraction(string actorId, string targetId, string type)
    {
        // type = view like, pass, superlike, block
        Guid parsedActorId = Guid.Parse(actorId);
        Guid parsedTargetId = Guid.Parse(targetId);
        Interactions interaction = Interactions.CreateInteractions(parsedActorId, parsedTargetId, type);
        await _interactionsRepository.UsersInteraction(interaction);
    }

    public async Task<IReadOnlyList<InteractionDto>> ListUserInteractions(string userId, string type, string? lastId, int sizePlusOne, CancellationToken ct)
    {
        Guid parsedUserId = Guid.Parse(userId);
        Guid? parsedLastId = Guid.TryParse(lastId, out var g) ? g : null;

        IEnumerable<Interactions> interacations = await _interactionsRepository.ListUserInteractionsByType(parsedUserId, type, parsedLastId, sizePlusOne + 1, ct);

        // Map “flat” → DTOs; materialize 1x
        var dtos = interacations.Select(i => new InteractionDto
        {
            Id = i.Id.ToString(),
            ActorId = i.ActorId.ToString(),
            TargetId = i.TargetId.ToString(),
            Type = i.Type
        }).ToArray();

        return dtos;
    }
}