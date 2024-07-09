using CgvMate.Data.Enums;
using CgvMate.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CgvMate.Api.Controllers;

[ApiController]
[Route("lotte/event")]
public class LotteEventController : ControllerBase
{
    private LotteService _service;

    public LotteEventController(LotteService service)
    {
        _service = service;
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetEventList([FromQuery] int? type)
    {
        if (type == null)
        {
            type = 20;
        }
        if (!Enum.IsDefined(typeof(LotteEventType), type))
        {
            return BadRequest();
        }
        var events = await _service.GetEventsAsync((LotteEventType)type);
        return Ok(events);
    }

    [HttpGet("cupon/list")]
    public async Task<IActionResult> GetCuponEventList()
    {

        var events = await _service.GetCuponEventsAsync();
        return Ok(events);
    }

    [HttpGet("giveaway/list")]
    public async Task<IActionResult> GetGiveawayEventList()
    {
        var events = await _service.GetGiveawayEventsAsync();
        return Ok(events);
    }

    [HttpGet("giveaway/model")]
    public async Task<IActionResult> GetGiveawayEventModel([FromQuery] string? event_id)
    {
        if (event_id == null)
        {
            return BadRequest();
        }
        var obj = await _service.GetLotteGiveawayEventModelAsync(event_id, true);
        return Ok(obj);
    }

    [HttpGet("giveaway/info")]
    public async Task<IActionResult> GetGiveawayInfoAsync([FromQuery] string? event_id,
                                                   [FromQuery] string? gift_id)
    {
        if ((event_id == null) || (gift_id == null))
        {
            return BadRequest();
        }
        var info = await _service.GetLotteGiveawayInfoAsync(event_id, gift_id);
        return Ok(info);
    }
}