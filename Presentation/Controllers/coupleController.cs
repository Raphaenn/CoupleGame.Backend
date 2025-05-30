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

    [HttpPost("/couple/start/")]
    public async Task<ActionResult<CoupleDto?>> CreateCouple([FromBody] CoupleRequest coupleRequest)
    {
        try
        {
            CoupleDto response = await _coupleAppService.StartCouple(coupleRequest.UserOneId, coupleRequest.CoupleType, coupleRequest.CoupleStatus);
            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    // todo - Add other member to couple
}