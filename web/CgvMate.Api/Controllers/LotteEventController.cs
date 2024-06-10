using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LotteMate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CgvMate.Api;

[ApiController]
[Route("lotte/event")]
public class LotteEventController : ControllerBase
{
    private LotteService _service;
    private AppDbContext db;

    public LotteEventController(LotteService service, AppDbContext dbContext)
    {
        _service = service;
        db = dbContext;
    }

    [HttpGet("list")]
    public async Task<string> GetEventList([FromQuery] int? type)
    {
        if (type == null)
        {
            type = 20;
        }
        if (!Enum.IsDefined(typeof(LotteEventType), type))
        {
            Console.WriteLine($"404 out of enum range");
            Response.StatusCode = 404;
            return $"404 not found";
        }
        var events = await _service.GetEventsAsync((LotteEventType)type);
        var eventIDs = events.Select(item => Int64.Parse(item.EventID)).ToList();

        // DB에서 한 번에 모든 뷰 아이템 조회
        var viewItems = await db.Views
            .Where(x => eventIDs.Contains(x.EventIndex))
            .ToDictionaryAsync(x => x.EventIndex, x => x.Count);

        foreach (var item in events)
        {
            if (viewItems.TryGetValue(Int64.Parse(item.EventID), out var count))
                item.Views = count;
            else
                item.Views = 0;
        }

        return JsonConvert.SerializeObject(events, Formatting.Indented);
    }

    [HttpGet("giveaway/model")]
    public async Task<string> GetGiveawayEventModel([FromQuery] string? event_id)
    {
        if (event_id == null)
        {
            Response.StatusCode = 404;
            return "404 not found";
        }
        var obj = await _service.GetLotteGiveawayEventModelAsync(event_id);

        var viewItem = await db.Views.FirstOrDefaultAsync(x => x.EventIndex == Int64.Parse(event_id));
        if (viewItem == null)
        {
            var item = new Views()
            {
                EventIndex = long.Parse(event_id),
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

        return JsonConvert.SerializeObject(obj, Formatting.Indented);
    }

    [HttpGet("giveaway/info")]
    public async Task<string> GetGiveawayInfoAsync([FromQuery] string? event_id,
                                                   [FromQuery] string? gift_id)
    {
        if ((event_id == null) || (gift_id == null))
        {
            Response.StatusCode = 404;
            return "404 not found";
        }
        var info = await _service.GetLotteGiveawayInfoAsync(event_id, gift_id);
        return JsonConvert.SerializeObject(info, Formatting.Indented);
    }
}
