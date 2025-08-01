using Domain.Entities;

namespace Domain.Interfaces;

public interface IInviteRepository
{
    Task CreateInvite(Invite invite);

    Task<Invite?> GetInviteByQuizEmail(Guid quizId, string email);
    
    Task<Invite?> GetInviteById(Guid id);

    Task AcceptInvite(Invite invite);
}