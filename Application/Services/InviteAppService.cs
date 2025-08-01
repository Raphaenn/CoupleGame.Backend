using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class InviteAppService : IInviteAppService
{
    private readonly IInviteRepository _inviteRepository;

    public InviteAppService(IInviteRepository inviteRepository)
    {
        _inviteRepository = inviteRepository;
    }

    public async Task<InviteDto> CreateInviteService(string quizId, string hostId, string email)
    {
        try
        {
            Guid parsedQuizId = Guid.Parse(quizId);
            Guid parsedHostId = Guid.Parse(hostId);
            Invite data = Invite.CreateInvite(parsedQuizId, parsedHostId, email);
            await _inviteRepository.CreateInvite(data);

            InviteDto invite = new InviteDto
            {
                Id = data.Id.ToString(),
                QuizId = data.QuizId.ToString(),
                Email = data.Email,
                Accepted = data.Accepted,
                CreatedAt = data.CreatedAt
            };

            return invite;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task GetInviteService(string quizId, string email)
    {
        throw new NotImplementedException();
    }

    public async Task AcceptInvite(string id)
    {
        try
        {
            Guid parsedId = Guid.Parse(id);
            Invite? getInvite = await _inviteRepository.GetInviteById(parsedId);

            if (getInvite == null)
            {
                throw new ApplicationException("Invalid invite");
            }
            
            getInvite.Accept();
            
            await _inviteRepository.AcceptInvite(getInvite);
            return;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}