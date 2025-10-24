using Domain.Entities;

namespace Domain.Interfaces;

public interface IInteractionsRepository
{
    Task UsersInteraction(Interactions interaction);
}