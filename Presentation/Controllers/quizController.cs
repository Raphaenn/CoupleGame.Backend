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

    public QuizController(IQuizAppService quizAppService)
    {
        _quizAppService = quizAppService;
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
}