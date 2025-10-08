using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Services;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RecommendationController : ControllerBase
{

    public record struct VoteRequest(string LadderId, string UserId, string User2Id, InteractionType Interaction, string Idp);
    public record struct GetRecRequest(decimal LScore, string UserId);
    
    // todo - apply app service
    private readonly IRecommendationAppService _recommendationAppService;

    public RecommendationController(IRecommendationAppService recommendationAppService)
    {
        _recommendationAppService = recommendationAppService;
    }
    
    [HttpGet("/")]
    public async Task<ActionResult> GetRecommendation([FromRoute] GetRecRequest req, CancellationToken ct)
    {
        try
        {
            var cursor = new RankingCursor(req.LScore, Guid.Parse(req.UserId));
            var res = await _recommendationAppService.GetRecommendationService(5, cursor, ct);
            return Ok(res);
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
            await _recommendationAppService.RecordVoteService(parsedLadder, parsedUserId, parsedUser2Id, req.Interaction, req.Idp, ct);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}