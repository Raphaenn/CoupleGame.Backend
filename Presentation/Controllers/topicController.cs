using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]/api")]
public class TopicController : ControllerBase
{
    private readonly ITopicAppService _topicAppService;

    public TopicController(ITopicAppService topicAppService)
    {
        this._topicAppService = topicAppService;
    }
    
    [HttpGet("/data/{id}")]
    public async Task<ActionResult<TopicDto>> GetTopicById([FromQuery] string id)
    {
        try
        {
            TopicDto response = await _topicAppService.GetTopicById(id);
            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(e?.Message);
        }
    }

    [HttpGet("/topic/list-all")]
    public async Task<ActionResult<TopicDto>> ListAllTopics()
    {
        try
        {
            List<TopicDto> response = await _topicAppService.ListAllTopics();
            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}