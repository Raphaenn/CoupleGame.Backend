using Domain.Entities;

namespace Domain.Interfaces;

public interface IInviteRepository
{
    Task CreateInvite(Invite invite);

    Task<List<Invite>>? GetInvitesByEmail(string email);
    
    Task<Invite?> GetInviteById(Guid id);

    Task AcceptInvite(Invite invite);

    Task DeleteInvite(Guid id);
}