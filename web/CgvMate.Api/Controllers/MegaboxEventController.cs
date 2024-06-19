using CgvMate.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CgvMate.Api.Controllers;

[ApiController]
[Route("megabox/event")]
public class MegaboxEventController : ControllerBase
{
    public MegaboxEventController(MegaboxEventService megaboxEventService)
    {
        _eventService = megaboxEventService;
    }

    private readonly MegaboxEventService _eventService;

    [HttpGet("giveaway/list")]
    public async Task<IActionResult> GetGiveawayEventList()
    {
        var events = await _eventService.GetGiveawayEventsAsync();

        return Ok(events);
    }

    [HttpGet("giveaway/detail")]
    public async Task<IActionResult> GetGiveawayInfoAsync([FromQuery] string? goodsNo)
    {
        if (goodsNo == null)
        {
            Response.StatusCode = 404;
            return BadRequest();
        }
        var info = await _eventService.GetGiveawayEventDetail(goodsNo);
        return Ok(info);
    }
}
