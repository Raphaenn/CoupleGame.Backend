using Application.Interfaces;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RecommendationController : ControllerBase
{
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
    
    [HttpGet("/ladder/lis")]
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
}