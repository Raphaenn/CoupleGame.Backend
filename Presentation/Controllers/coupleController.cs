using Api.Requests;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("couple")]
public class CoupleController : ControllerBase
{
    public record IAddCouple(string CoupleId, string UserId);
    private readonly ICoupleAppService _coupleAppService;

    public CoupleController(ICoupleAppService coupleAppService)
    {
        _coupleAppService = coupleAppService;
    }

    [HttpPost("start")]
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
    
    [HttpPost("create/temporary")]
    public async Task<ActionResult<CoupleDto?>> CreateTempCouple([FromBody] CreateTemCoupleReq coupleRequest, CancellationToken ct)
    {
        try
        {
            if (coupleRequest.UserId1 is null)
            {
                throw new Exception("Invalid user");
            }
            CoupleDto response = await _coupleAppService.CreateTempCouple(coupleRequest.UserId1, ct);
            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost("search/temporary")]
    public async Task<ActionResult<CoupleDto?>> GetTempCouple([FromBody] SearchTemCoupleReq coupleRequest)
    {
        try
        {
            if (coupleRequest.UserId1 is null || coupleRequest.UserId2 is null)
            {
                throw new Exception("Users");
            }
            CoupleDto response = await _coupleAppService.GetTempCouple(coupleRequest.UserId1, coupleRequest.UserId2);
            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("add-member")]
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

    [HttpGet("get-partner/{id}")]
    public async Task<ActionResult<CoupleDto>> GetCouplePartner([FromRoute] Guid id)
    {
        try
        {
            CoupleDto couple = await _coupleAppService.GetCouplePartner(id);
            return Ok(couple);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}