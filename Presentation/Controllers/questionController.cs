using System.Security.Claims;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]/api")]
public class QuestionController : ControllerBase
{
    private readonly IQuestionAppService _questionAppService;

    public QuestionController(IQuestionAppService questionAppService)
    {
        _questionAppService = questionAppService;
    }

    [HttpGet("/question/data/{questionId}")]
    public async Task<ActionResult<QuestionDto>> GetQuestion([FromQuery] string questionId)
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

    [HttpGet("/question/search/{topicId}")]
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
    
    [Authorize]
    [HttpGet("/question/random/{topicId}")]
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
}