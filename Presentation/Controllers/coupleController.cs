using Api.Requests;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]/api")]
public class CoupleController : ControllerBase
{
    private readonly ICoupleAppService _coupleAppService;

    public CoupleController(ICoupleAppService coupleAppService)
    {
        _coupleAppService = coupleAppService;
    }

    [HttpPost("/couple/create/")]
    public async Task<ActionResult<CoupleDto?>> CreateCouple([FromBody] CoupleRequest coupleRequest)
    {
        try
        {
            CoupleDto response = await _coupleAppService.CreateRelationship(userOne: coupleRequest.UserOneId, userTwo: coupleRequest.UserTwoId);
            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}