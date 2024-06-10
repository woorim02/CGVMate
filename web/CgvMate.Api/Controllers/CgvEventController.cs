using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CgvMate.Api.Controllers;

[ApiController]
[Route("cgv/event")]
public class CgvEventController : ControllerBase
{
    private CgvService _service;
    private AppDbContext db;

    public CgvEventController(CgvService service, AppDbContext dbContext)
    {
        _service = service;
        db = dbContext;
    }

    [HttpGet("list")]
    public async Task<string> GetEventList([FromQuery] int? type)
    {
        if (type == null)
        {
            type = 1;
        }
        if (!Enum.IsDefined(typeof(EventType), type))
        {
            Console.WriteLine($"404 out of enum range");
            Response.StatusCode = 404;
            return $"404 not found {JsonConvert.SerializeObject(EventType.Special)}";
        }
        var events = await _service.Event.GetEvents((EventType)type);
        return JsonConvert.SerializeObject(events, Formatting.Indented);
    }

    [HttpGet("detail/{eventId}")]
    public async Task<string> GetEventDetailAsync([FromRoute] string eventId)
    {
        var images = await _service.Event.GetEventImgSrcAsync(eventId);
        return JsonConvert.SerializeObject(images, Formatting.Indented);
    }

    [HttpGet("giveaway/list")]
    public async Task<string> GetGiveawayEventList()
    {
        var events = await _service.Event.GetGiveawayEventsAsync();
        var eventIndexes = events.Select(item => Int64.Parse(item.EventIndex)).ToList();

        // DB에서 한 번에 모든 viewItem 조회
        var viewItems = await db.Views
            .Where(x => eventIndexes.Contains(x.EventIndex))
            .ToDictionaryAsync(x => x.EventIndex, x => x.Count);

        foreach (var item in events)
        {
            if (viewItems.TryGetValue(Int64.Parse(item.EventIndex), out var count))
                item.Views = count;
            else
                item.Views = 0;
        }
        return JsonConvert.SerializeObject(events, Formatting.Indented);
    }

    [HttpGet("giveaway/model")]
    public async Task<string> GetGiveawayEventModel([FromQuery] string? eventIndex)
    {
        if (eventIndex == null)
        {
            Response.StatusCode = 404;
            return "404 not found";
        }

        var viewItem = await db.Views.FirstOrDefaultAsync(x => x.EventIndex == Int64.Parse(eventIndex));
        if (viewItem == null)
        {
            var item = new Views()
            {
                EventIndex = long.Parse(eventIndex),
                Count = 1
            };
            await db.Views.AddAsync(item);
        }
        else
        {
            viewItem.Count++;
            db.Views.Update(viewItem);
        }
        await db.SaveChangesAsync();

        var model = await _service.Event.GetGiveawayEventModelAsync(eventIndex);
        return JsonConvert.SerializeObject(model, Formatting.Indented);
    }

    [HttpGet("giveaway/info")]
    public async Task<string> GetGiveawayInfoAsync([FromQuery] string? giveawayIndex,
                                                   [FromQuery] string? areaCode)
    {
        if (giveawayIndex == null)
        {
            Response.StatusCode = 404;
            return "404 not found";
        }
        var info = await _service.Event.GetGiveawayInfoAsync(giveawayIndex, areaCode ?? "");
        return JsonConvert.SerializeObject(info, Formatting.Indented);
    }
}
