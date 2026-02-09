using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("/pce")]
public class PceController : ControllerBase
{
    private readonly IPceAppServices _pceAppServices;
    
    public record struct SaveAnswerBody(Guid UserId, Guid PceId, Guid QuestionId, Guid TopicId, string Content);

    public record struct GetPceQuizReq(Guid CoupleId);
    public record struct InitiatePceReq(Guid CoupleId);

    public PceController(IPceAppServices pceAppServices)
    {
        _pceAppServices = pceAppServices;
    }

    [HttpPost("init")]
    public async Task<ActionResult> InitCouplePce([FromBody] InitiatePceReq req, CancellationToken ct)
    {
        try
        {
            await _pceAppServices.InitNewPce(req.CoupleId, ct);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("save-answer")]
    public async Task<ActionResult> SaveUserAnswers([FromBody] SaveAnswerBody req, CancellationToken ct)
    {
        try
        {
            await _pceAppServices.SaveAnswers(req.UserId, req.PceId, req.QuestionId, req.TopicId, req.Content, ct);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet("pce-quiz/{id}")]
    public async Task<ActionResult> GetPceQuiz([FromRoute] Guid id, CancellationToken ct)
    {
        try
        {
            PceDto pce =  await _pceAppServices.GetPceByCouple(id, ct);
            return Ok(pce);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet("result/{id}")]
    public async Task<ActionResult<List<PceResultDto>>> GetPceResults([FromRoute] Guid id, CancellationToken ct)
    {
        try
        {
            List<PceResultDto> result = await _pceAppServices.GetPceResult(id, ct);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}