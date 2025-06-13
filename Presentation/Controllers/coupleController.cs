using Api.Requests;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]/api")]
public class CoupleController : ControllerBase
{
    public record IAddCouple(string CoupleId, string UserId);
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

    [HttpPost("/couple/add-member")]
    public async Task<ActionResult> AddMember([FromBody] IAddCouple body)
    {
        try
        {
            await _coupleAppService.AddSecondMember(body.CoupleId, body.UserId);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}