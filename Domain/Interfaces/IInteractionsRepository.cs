using Domain.Entities;

namespace Domain.Interfaces;

public interface IInteractionsRepository
{
    Task UsersInteraction(Interactions interaction);
    
    Task<IReadOnlyList<Interactions>> ListUserInteractionsByType(Guid userId, string type, Guid? lastId, int limitPlusOne, CancellationToken ct);
}