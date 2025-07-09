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
    private readonly IUserPrefrencesService _userPreferencesService;

    public UserPreferencesController(IUserPrefrencesService userPreferencesService)
    {
       _userPreferencesService = userPreferencesService; 
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

            UserPreferencesDto createdUser = await _userPreferencesService.CreateUserPreferences(userRequest);
            return Ok(createdUser);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}