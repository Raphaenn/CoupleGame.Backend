using System.Security.Claims;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("question")]
public class QuestionController : ControllerBase
{
    public record PagedResponse<T>(IReadOnlyList<T> Data, int Total);
    private readonly IQuestionAppService _questionAppService;

    public QuestionController(IQuestionAppService questionAppService)
    {
        _questionAppService = questionAppService;
    }

    [HttpGet("data/{questionId}")]
    public async Task<ActionResult<QuestionDto>> GetQuestion([FromRoute] string questionId)
    {
        try
        {
            QuestionDto response = await _questionAppService.GetQuestion(questionId); 
            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("search/{topicId}")]
    public async Task<ActionResult<List<QuestionDto>>> ListQuestionsByTopic(string topicId)
    {
        try
        {
            List<QuestionDto> response = await _questionAppService.GetQuestionsByTopic(topicId);
            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet("random/{topicId}")]
    public async Task<ActionResult<List<QuestionDto>>> GetRandomQuestion([FromRoute] string topicId, [FromQuery] string? quizId)
    {
        try
        {
            foreach (var d in User.Claims)
            {
                Console.WriteLine(d);
            }

            QuestionDto response = await _questionAppService.RandomQuestion(topicId, quizId);
            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("all")]
    public async Task<ActionResult<ActionResult<PagedResponse<QuestionDto>>>> GetAllQuestions(CancellationToken ct)
    {
        try
        {
            List<QuestionDto> res = await _questionAppService.ListAllQuestions(ct);
            var result = new Dictionary<string, object>
            {
                { "data", res },
                { "total", res.Count }
            };
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}