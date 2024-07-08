using CgvMate.Data.Entities.Cgv;
using CgvMate.Services.Apis;
using CgvMate.Services.Repos;
using System.Data;

namespace CgvMate.Services;

public class CgvEventService : CgvServiceBase
{
    private readonly HttpClient _client;
    private readonly ICgvGiveawayEventRepository _giveawayEventRepository;
    private readonly CgvApi _api;

    public CgvEventService(HttpClient client,
                           ICgvGiveawayEventRepository giveawatEventRepository,
                           string iv,
                           string key) : base(iv, key)
    {
        _client = client;
        _giveawayEventRepository = giveawatEventRepository;
        _api = new CgvApi(client, base.Decrypt);
    }

    public async Task<List<Event>> GetEvents(CgvEventType type)
    {
        var list = await _api.GetEvents(type);
        return list;
    }

    public async Task<List<Event>> GetCuponEventsAsync()
    {
        var events = await _api.GetEvents(CgvEventType.Movie);
        var cuponEvents = events
            .Where(e => e.EventName.Contains("스피드 쿠폰")
                     || e.EventName.Contains("선착순 무료 쿠폰"))
            .ToList();
        return cuponEvents;
    }

    #region Giveaway
    public async Task<List<GiveawayEvent>> GetGiveawayEventsAsync()
    {
        // 이벤트 리스트 불러오기
        var list = await _api.GetGiveawayEventsAsync();
        var kvp = list.ToDictionary(x => x.EventIndex);

        // 뷰 리스트 불러오기, 뷰 주입
        var views = await _giveawayEventRepository.GetViewsAsync(kvp.Select(x => x.Value.EventIndex));
        foreach (var view in views)
        {
            kvp[view.Key].Views = view.Value;
        }

        // 뷰 리스트에 존재하지 않는 이벤트 엔티티 추가
        var updateList = new List<GiveawayEvent>();
        foreach (var item in kvp)
        {
            if (!views.ContainsKey(item.Key))
            {
                updateList.Add(item.Value);
            }
        }
        await _giveawayEventRepository.AddAsync(updateList);
        return [.. kvp.Values];
    }

    public async Task<GiveawayEventModel> GetGiveawayEventModelAsync(string eventIndex)
    {
        // 모델 불러오기
        var model = await _api.GetGiveawayEventModelAsync(eventIndex);

        // 모델 조회수 높이기
        await _giveawayEventRepository.UpdateViewAsync(eventIndex);

        return model;
    }

    public async Task<GiveawayEventDetail> GetGiveawayInfoAsync(string giveawayIndex, string areaCode = "")
    {
        var info = await _api.GetGiveawayInfoAsync(giveawayIndex, areaCode);
        return info;
    }

    #endregion
}