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
            Password = user.Senha,
            Birthdate = user.DataNascimento,
            Height = user.Altura,
            Weight = user.Peso,
         };

         UserDto createdUser = await _userAppService.CreateUser(userRequest);
         return Ok(createdUser);
      }
      catch (Exception e)
      {
         return BadRequest(e.Message);
      }
   }
}