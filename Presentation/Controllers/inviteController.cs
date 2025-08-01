using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public class InviteController : ControllerBase
{
    public record struct ICreateRequest(string HostId, string QuizId, string Email);
    
    private readonly IInviteAppService _inviteAppService;

    public InviteController(IInviteAppService inviteAppService)
    {
        this._inviteAppService = inviteAppService;
    }
    
    [HttpPost("/invite/create")]
    public async Task<ActionResult<InviteDto>> CreateInviteController([FromBody] ICreateRequest req)
    {
        try
        {
            InviteDto res = await _inviteAppService.CreateInviteService(req.HostId, req.QuizId, req.Email);
            return Ok(res);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("/invite/accept")]
    public async Task<ActionResult> AcceptInviteController(string inviteId)
    {
        try
        {
            await _inviteAppService.AcceptInvite(inviteId);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    // [HttpGet("/invite/show")]
}