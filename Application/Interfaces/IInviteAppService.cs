using Application.Dtos;

namespace Application.Interfaces;

public interface IInviteAppService
{
    Task<InviteDto> CreateInviteService(string quizId, string hostId, string email);

    Task<InviteDto?> GetInviteByQuizEmail(string quizId, string email);

    Task AcceptInvite(string inviteId);
}