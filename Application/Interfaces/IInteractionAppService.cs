namespace Application.Interfaces;

public interface IInteractionAppService
{
    Task CreateUsersInteraction(string actorId, string targetId, string type);
}