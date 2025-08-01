using Api.Requests;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]/api")]
public class UserPreferencesController : ControllerBase
{
    private readonly IUserPreferencesAppService _userPreferencesAppService;

    public UserPreferencesController(IUserPreferencesAppService userPreferencesAppService)
    {
        _userPreferencesAppService = userPreferencesAppService; 
    }
    
    [HttpPost("/user/create-preferences")]
    public async Task<ActionResult<UserPreferencesDto>> CreateNewUser([FromBody] UserPreferencesRequest user)
    {
        try
        {
            UserPreferencesDto userRequest = new UserPreferencesDto
            {
                UserId = string.IsNullOrEmpty(user.UserId) ? Guid.Empty : Guid.Parse(user.UserId),
            };

            UserPreferencesDto createdUser = await _userPreferencesAppService.CreateUserPreferences(userRequest);
            return Ok(createdUser);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("/user/preferences/{id}")]
    public async Task<ActionResult<UserPreferencesDto?>> GetUserPreferences([FromRoute] string id)
    {
        try
        {
            return Ok(id);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}