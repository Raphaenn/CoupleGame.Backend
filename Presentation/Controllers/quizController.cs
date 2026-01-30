using Api.Requests;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("quiz")]
public class QuizController : ControllerBase
{
    private readonly IQuizAppService _quizAppService;

    public record ResultRequest(string QuizId, string A1Id, string A2Id);

    public QuizController(IQuizAppService quizAppService)
    {
        _quizAppService = quizAppService;
    }
    
    [HttpPost("start")]
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
    
    [HttpPost("create")]
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
    
    [HttpGet("quiz/get/{coupleId}")]
    public async Task<ActionResult<List<QuizDto>>> GetQuizByCoupleId([FromRoute] string coupleId)
    {
        try
        {
            List<QuizDto>? response = await _quizAppService.ListQuizByCoupleId(coupleId);
            
            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost("update")]
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
    
    [HttpPost("answer-question")]
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

    [HttpGet("invited/{id}")]
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

    // List completed quizzes by couple id
    [HttpGet("list/completed/{id}")]
    public async Task<ActionResult<List<QuizDto>>> GetCompletedQuiz([FromRoute] string id)
    {
        try
        {
            var q = await _quizAppService.ListCompletedQuizByCoupleId(id);
            return Ok(q);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    // todo - List completed quizzes by user id
    // [HttpGet("/quiz/list/completed/{id}")]
    // public async Task<ActionResult<List<QuizDto>>> GetCompletedQuiz([FromRoute] string id)
    // {
    //     try
    //     {
    //         await _quizAppService.ListQuizByCoupleId()
    //         return Ok();
    //     }
    //     catch (Exception e)
    //     {
    //         return BadRequest(e.Message);
    //     }
    // }
    
    [HttpGet("result/{id}")]
    public async Task<ActionResult<QuizDto>> GetQuizResult([FromRoute] string id)
    {
        try
        {
            var q = await _quizAppService.GetResult(id);
            return Ok(q);
        } catch (Exception e) {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("result/stats/{id}")]
    public async Task<ActionResult<QuizDto>> GetQuizResultStats([FromRoute] string id)
    {
        try
        {
            var q = await _quizAppService.GetQuizStats(id);
            return Ok(q);
        } catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("global/stats/{id}")]
    public async Task<ActionResult<QuizStatsDto>> GetGlobalStatusByCouple([FromRoute] string id)
    {
        try
        {
            var result = await _quizAppService.GetGlobalQuizStats(id);
            return Ok(result);
        } catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost("change-status")]
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
    
    [HttpGet("open-list/{userId}")]
    public async Task<ActionResult<QuizDto>> ListOpenQuiz([FromRoute] string userId)
    {
        try
        {
            var q = await _quizAppService.ListOpenQuiz(userId);
            return Ok(q);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}