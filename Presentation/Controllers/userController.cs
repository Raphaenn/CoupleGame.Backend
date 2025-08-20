using Api.Requests;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]/api")]
public class UserController : ControllerBase
{
   private readonly IUserAppService _userAppService;

   public record struct UserSearchRequest(string Id);

   public UserController(IUserAppService userAppService)
   {
      _userAppService = userAppService;
   }

   [HttpPost("/user/create")]
   public async Task<ActionResult<UserDto>> CreateNewUser([FromBody] UserRequest user)
   {
      try
      {
         UserDto userRequest = new UserDto
         {
            Name = user.Nome,
            Email = user.Email,
         };

         UserDto createdUser = await _userAppService.CreateUser(userRequest);
         return Ok(createdUser);
      }
      catch (Exception e)
      {
         return BadRequest(e.Message);
      }
   }
   
   [HttpPost("/user/search")]
   public async Task<ActionResult<UserDto>> SearchUserInfo([FromBody] UserSearchRequest req)
   {
      try
      {
         UserDto user = await _userAppService.SearchUserService(req.Id);
         return Ok(user);
      }
      catch (Exception e)
      {
         return BadRequest(e.Message);
      }
   }
}