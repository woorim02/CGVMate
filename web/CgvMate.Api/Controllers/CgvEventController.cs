using CgvMate.Services;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace CgvMate.Api.Controllers;

[ApiController]
[Route("cgv/event")]
public class CgvEventController : ControllerBase
{
    private CgvEventService _service;

    public CgvEventController(CgvEventService service)
    {
        _service = service;
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetEventList([FromQuery] int? type)
    {
        if (type == null)
        {
            type = 1;
        }
        if (!Enum.IsDefined(typeof(CgvEventType), type))
        {
            return BadRequest();
        }
        var events = await _service.GetEvents((CgvEventType)type);
        return Ok(events);
    }

    [HttpGet("giveaway/list")]
    public async Task<IActionResult> GetGiveawayEventList()
    {
        var events = await _service.GetGiveawayEventsAsync();

        return Ok(events);
    }

    [HttpGet("giveaway/model")]
    public async Task<IActionResult> GetGiveawayEventModel([FromQuery] string? eventIndex)
    {
        if (eventIndex == null)
        {
            Response.StatusCode = 404;
            return BadRequest();
        }

        var model = await _service.GetGiveawayEventModelAsync(eventIndex);
        return Ok(model);
    }

    [HttpGet("giveaway/info")]
    public async Task<IActionResult> GetGiveawayInfoAsync([FromQuery] string? giveawayIndex,
                                                         [FromQuery] string? areaCode)
    {
        if (giveawayIndex == null)
        {
            Response.StatusCode = 404;
            return BadRequest();
        }
        var info = await _service.GetGiveawayInfoAsync(giveawayIndex, areaCode ?? "");
        return Ok(JsonConvert.SerializeObject(info));
    }
}