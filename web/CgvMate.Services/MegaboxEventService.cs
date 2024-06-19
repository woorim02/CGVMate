using CgvMate.Data.Entities.Megabox;

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
            var detail = GetGiveawayEventDetail(item.ID);
            if (detail != null)
            {
                break;
            }
            await _giveawayEventRepository.DeleteAsync(item.ID);
        }

        var dsc = events.OrderByDescending(x => x.ID);
        foreach (var item in dsc)
        {
            int id;
            bool parseResult = int.TryParse(item.ID.Replace("FG", ""), out id);
            if (!parseResult)
            {
                throw new Exception("메가박스 웹 구조 변경으로 파싱 실패");
            }
            var detail = await GetGiveawayEventDetail($"FG{id:D6}");
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
