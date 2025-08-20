using Api.Requests;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public class InviteController : ControllerBase
{
    private readonly IInviteAppService _inviteAppService;

    public InviteController(IInviteAppService inviteAppService)
    {
        this._inviteAppService = inviteAppService;
    }
    
    [HttpPost("/invite/create")]
    public async Task<ActionResult<InviteDto>> CreateInviteController([FromBody] CreateRequest req)
    {
        try
        {
            InviteDto res = await _inviteAppService.CreateInviteService(req.QuizId, req.HostId, req.Email);
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

    [HttpGet("/invite/show")]
    public async Task<ActionResult<InviteDto>> GetInviteByEmail([FromQuery] GetInviteRequest req)
    {
        try
        {
            List<InviteDto>? res = await _inviteAppService.InvitesByEmail(req.Email);

            if (res == null)
            {
                return NotFound("Invite not found");
            }
            return Ok(res);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}