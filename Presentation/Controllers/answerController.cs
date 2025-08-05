using Api.Requests;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]/api")]
public class AnswerController : ControllerBase
{
    private readonly IAnswerAppService _answerAppService;

    public AnswerController(IAnswerAppService answerAppService)
    {
        _answerAppService = answerAppService;
    }

    [HttpPost("/answer/create")]
    public async Task<ActionResult<AnswerDto>> InitAnswer([FromBody] InitAnswerRequest request)
    {
        try
        {
            AnswerDto answer = await _answerAppService.AnswerQuiz(userId: request.UserId, quizId: request.QuizId, answer1: request.Answer1, answer2: request.Answer2, answer3: request.Answer3, answer4: request.Answer4, answer5: request.Answer5, answer6: request.Answer6);
            return Ok(answer);
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

    [HttpPost("/answers/completed")]
    public async Task<ActionResult<CompletedAnswers>> GetCompletedAnswers([FromBody] CompletedAnswersRequest req)
    {
        try
        {
            CompletedAnswers res = await _answerAppService.GetCompleteAnswerByQuiz(req.QuizId, req.UserId);
            return Ok(res);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}