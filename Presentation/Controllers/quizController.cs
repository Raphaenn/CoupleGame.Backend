using Api.Requests;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]/api")]

public class QuizController : ControllerBase
{
    private readonly IQuizAppService _quizAppService;

    public record ResultRequest(string QuizId, string A1Id, string A2Id);

    public QuizController(IQuizAppService quizAppService)
    {
        _quizAppService = quizAppService;
    }
    
    [HttpPost("/quiz/start")]
    public async Task<ActionResult<QuizDto>> StartNewQuiz([FromBody] StartQuizRequest request)
    {
        try
        {
            QuizDto? response = await _quizAppService.StartQuiz(coupleId: request.CoupleId, questionId: request.QuestionId);
            
            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost("/quiz/create")]
    public async Task<ActionResult<QuizDto>> CreateQuiz([FromBody] QuizDto request)
    {
        try
        {
            // await _quizAppService.CreateNewQuiz(request);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet("/quiz/get/{coupleId}")]
    public async Task<ActionResult<QuizDto?>> GetQuizByCoupleId([FromQuery] string coupleId)
    {
        try
        {
            // QuizDto? response = await _quizAppService.GetQuizByCouple(coupleId);
            
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost("/quiz/update")]
    public async Task<ActionResult<QuizDto>> UpdateQuiz([FromBody] QuizRequest request)
    {
        try
        {
            QuizDto? response = await _quizAppService.UpdateQuiz(quizId: request.QuizId, questionId: request.QuestionId);
            
            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost("/quiz/answer-question")]
    public async Task<ActionResult<QuizDto>> UpdateQuiz([FromBody] QuizAnswerRequest request)
    {
        try
        {
            AnswerDto answer = await _quizAppService.AnswerQuizQuestion(request.UserId, request.QuizId, request.Answer);
            
            return Ok(answer);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("/quiz/invited/{id}")]
    public async Task<ActionResult<QuizDto>> GetInvitedQuiz([FromRoute] string id)
    {
        try
        {
            QuizDto quiz = await _quizAppService.GetInviteQuiz(id);
            return quiz;
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("/quiz/completed/{id}")]
    public async Task<ActionResult<QuizDto>> GetCompletedQuiz([FromRoute] string id)
    {
        try
        {
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet("/quiz/result/{id}")]
    public async Task<ActionResult<QuizDto>> GetQuizResult([FromRoute] string id)
    {
        try
        {
            var q = await _quizAppService.GetResult(id);
            return Ok(q);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost("/quiz/change-status")]
    public async Task<ActionResult<QuizDto>> UpdateStatus([FromBody] UpdateQuizRequest req)
    {
        try
        {
            var q = await _quizAppService.UpdateQuizStatus(req.QuizId, req.Status);
            return Ok(q);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}