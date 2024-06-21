using CgvMate.Data.Entities.Megabox;
using CgvMate.Services.Repos;

namespace CgvMate.Services;

public class MegaboxEventService
{
    public MegaboxEventService(HttpClient httpClient, IMegaboxGiveawayEventRepository giveawayEventRepository)
    {
        _api = new MegaboxApi(httpClient);
        _giveawayEventRepository = giveawayEventRepository;
    }

    private readonly MegaboxApi _api;
    private readonly IMegaboxGiveawayEventRepository _giveawayEventRepository;

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
