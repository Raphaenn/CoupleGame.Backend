using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class InteractionsController : ControllerBase
{
    public record struct CreateInteractionReq(string ActorId, string TargetId, string Type);
    public record ListUserInteractionsReq(string Type, string? LastId, int Size);
    
    private readonly IInteractionAppService _interactionAppService;

    public InteractionsController(IInteractionAppService interactionAppService)
    {
        _interactionAppService = interactionAppService;
    }
    
    [HttpPost("create")]
    public async Task<ActionResult> CreateUsersInteraction([FromBody] CreateInteractionReq req, CancellationToken ct)
    {
        try
        {
            await _interactionAppService.CreateUsersInteraction(req.ActorId, req.TargetId, req.Type);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("list/{id}")]
    public async Task<ActionResult<IEnumerable<InteractionDto>>> ListSaves([FromRoute] string id, [FromQuery] ListUserInteractionsReq req, CancellationToken ct)
    {
        try
        {
             IReadOnlyList<InteractionDto> res = await _interactionAppService.ListUserInteractions(id, req.Type, req.LastId, req.Size, ct);
            return Ok(res);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}