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
        Guid parsedActorId = Guid.Parse(actorId);
        Guid parsedTargetId = Guid.Parse(targetId);
        Interactions interaction = Interactions.CreateInteractions(parsedActorId, parsedTargetId, "like");
        await _interactionsRepository.UsersInteraction(interaction);
    }
}