using Application.Dtos;

namespace Application.Interfaces;

public interface IInviteAppService
{
    Task<InviteDto> CreateInviteService(string quizId, string hostId, string email);

    Task GetInviteService(string quizId, string email);

    Task AcceptInvite(string inviteId);
}