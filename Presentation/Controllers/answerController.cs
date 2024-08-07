using Presentation.Requests;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("[controller]/api")]
public class AnswerController : ControllerBase
{
    private readonly IAnswerAppService _answerAppService;

    public AnswerController(IAnswerAppService answerAppService)
    {
        _answerAppService = answerAppService;
    }

    [HttpPost("/answer/init")]
    public async Task<ActionResult<AnswerDto>> InitAnswer([FromBody] InitAnswerRequest request)
    {
        try
        {
            await _answerAppService.CreateNewAnswer(userId: request.UserId, quizId: request.QuizId, answer: request.Answer);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPut("/answer/update")]
    public async Task<ActionResult<AnswerDto>> UpdateAnswer([FromBody] UpdateAnswerRequest request)
    {
        try
        {
            await _answerAppService.UpdateAnswerById(id: request.Id, answer: request.Answer);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}