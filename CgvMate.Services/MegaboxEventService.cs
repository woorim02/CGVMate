using CgvMate.Data.Entities.Megabox;
using CgvMate.Services.Apis;
using CgvMate.Services.Repos;
using System.Runtime.InteropServices;

namespace CgvMate.Services;

public class MegaboxEventService
{
    public MegaboxEventService(HttpClient httpClient,
                               IMegaboxGiveawayEventRepository giveawayEventRepository,
                               IMegaboxCuponEventRepository cuponEventRepository)
    {
        _client = httpClient;
        _api = new MegaboxApi(httpClient);
        _giveawayEventRepository = giveawayEventRepository;
        _cuponEventRepository = cuponEventRepository;
    }

    private readonly HttpClient _client;
    private readonly MegaboxApi _api;
    private readonly IMegaboxGiveawayEventRepository _giveawayEventRepository;
    private readonly IMegaboxCuponEventRepository _cuponEventRepository;

    public async Task<List<CuponEvent>> GetCuponEventsAsync()
    {
        var cuponEvents = await _api.GetCuponEventsAsync();
        var dbEvents = await _cuponEventRepository.GetCuponEventsAsync();
        var kvp = dbEvents.ToDictionary(e => e.EventNo);
        var updateList = new List<CuponEvent>();
        var finalList = new List<CuponEvent>();
        foreach (var e in cuponEvents)
        {
            if (kvp.ContainsKey(e.EventNo))
            {
                finalList.Add(kvp[e.EventNo]);
            }
            else
            {
                var cuponEvent = new CuponEvent()
                {
                    EventNo = e.EventNo,
                    Date = e.Date,
                    ImageUrl = e.ImageUrl,
                    Title = e.Title,
                    StartDateTime = await _api.GetCuponStartDateTimeAsync(e.EventNo)
                };
                finalList.Add(cuponEvent);
                updateList.Add(cuponEvent);
            }
        }
        if (updateList.Count > 0)
        {
            await _cuponEventRepository.AddCuponEventsAsync(updateList);
        }
        return finalList;
    }

    public async Task<List<GiveawayEvent>> GetGiveawayEventsAsync()
    {
        var events = (await _giveawayEventRepository.GetAllAsync()).ToList();
        if (events.Count == 0)
        {
            return Enumerable.Empty<GiveawayEvent>().ToList();
        }

        var asd = events.OrderBy(x => x.ID);
        foreach(var item in asd)
        {
            var detail = await _api.GetGiveawayEventDetailAsync(item.ID);
            if (detail != null)
            {
                break;
            }
            await _giveawayEventRepository.DeleteAsync(item.ID);
        }

        var latestItem = events.OrderByDescending(x => x.ID).FirstOrDefault();
        var latestItemId = int.Parse(latestItem.ID.Replace("FG", ""));
        while(true)
        {
            var detail = await _api.GetGiveawayEventDetailAsync($"FG{++latestItemId:D6}");
            if (detail != null)
            {
                await _giveawayEventRepository.AddAsync(new GiveawayEvent()
                {
                    ID = detail.ID,
                    Title = detail.Title,
                    ViewCount = 0,
                });
            }
            else
            {
                break;
            }
        }
        events = (await _giveawayEventRepository.GetAllAsync()).ToList();
        return events;
    }

    public async Task<GiveawayEventDetail?> GetGiveawayEventDetail(string giveawayId)
    {
        var detail = await _api.GetGiveawayEventDetailAsync(giveawayId);

        await _giveawayEventRepository.UpdateViewAsync(giveawayId);

        return detail;
    }
}
