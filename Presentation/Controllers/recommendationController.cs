using Application.Interfaces;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RecommendationController : ControllerBase
{

    public record struct VoteRequest(string LadderId, string UserId, string User2Id, string Winner, string Idp);
    
    // todo - apply app service
    private readonly IRecommendationAppService _recommendationAppService;

    public RecommendationController(IRecommendationAppService recommendationAppService)
    {
        _recommendationAppService = recommendationAppService;
    }
    
    [HttpGet("/ladder/{id}")]
    public async Task<ActionResult> GetRecommendation([FromRoute] string id, CancellationToken ct)
    {
        try
        {
            Ladder response = await _recommendationAppService.GetLadderById(id, ct);
            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet("/ranking")]
    public async Task<ActionResult> GetList(CancellationToken ct)
    {
        try
        {
            LadderId ladderId = new LadderId(Guid.Parse("550e8400-e29b-41d4-a716-446655440000"));
            var list = await _recommendationAppService.GetRecommendationService(ladderId);
            return Ok(list);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("/vote/register")]
    public async Task<ActionResult> RegisterVote([FromBody] VoteRequest req, CancellationToken ct)
    {
        try
        {
            LadderId parsedLadder = new LadderId(Guid.Parse(req.LadderId));
            Guid parsedUserId = Guid.Parse(req.UserId);
            Guid parsedUser2Id = Guid.Parse(req.User2Id);
            Guid parsedWinner = Guid.Parse(req.Winner);

            await _recommendationAppService.RecordVoteService(parsedLadder, parsedUserId, parsedUser2Id, parsedWinner, req.Idp, ct);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}