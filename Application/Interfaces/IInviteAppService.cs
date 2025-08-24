using Application.Dtos;

namespace Application.Interfaces;

public interface IInviteAppService
{
    Task<InviteDto> CreateInviteService(string quizId, string hostId, string email);

    Task<List<InviteDto>> InvitesByEmail(string email);

    Task AcceptInvite(string inviteId);

    Task DeleteInvite(string id);
}