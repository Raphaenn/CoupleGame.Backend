using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]/api")]
public class InteractionsController : ControllerBase
{
    public record struct CreateInteractionReq(string ActorId, string TargetId, string Type);
    
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
}