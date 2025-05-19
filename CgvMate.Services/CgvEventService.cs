using CgvMate.Data.Entities.Cgv;
using CgvMate.Services.Apis;
using CgvMate.Services.Repos;
using System.Data;

namespace CgvMate.Services;

public class CgvEventService
{
    private readonly HttpClient _client;
    private readonly ICgvGiveawayEventRepository _giveawayEventRepository;
    private readonly ICgvCuponEventRepository _cuponEventRepository;
    private readonly CgvApi _api;

    public CgvEventService(HttpClient client,
                           ICgvGiveawayEventRepository giveawatEventRepository,
                           ICgvCuponEventRepository cuponEventRepository,
                           string iv,
                           string key)
    {
        _client = client;
        _giveawayEventRepository = giveawatEventRepository;
        _cuponEventRepository = cuponEventRepository;
        _api = new CgvApi(client, null);
    }

    public async Task<List<Event>> GetEvents(CgvEventType type)
    {
        var list = await _api.GetEvents(type);
        return list;
    }

    public async Task<List<CuponEvent>> GetCuponEventsAsync()
    {
        var events = await _api.GetEvents(CgvEventType.Movie);
        var cuponEvents = events
            .Where(e => e.EventName.Contains("스피드 쿠폰")
                     || e.EventName.Contains("선착순 무료")
                     || e.EventName.Contains("서프라이즈"))
            .ToList();
        var dbEvents = await _cuponEventRepository.GetCuponEventsAsync();
        var kvp = dbEvents.ToDictionary(e => e.EventId);
        var updateList = new List<CuponEvent>();
        var finalList = new List<CuponEvent>();
        foreach (var e in cuponEvents)
        {
            if (kvp.ContainsKey(e.EventId))
            {
                finalList.Add(kvp[e.EventId]);
            }
            else
            {
                var dateTime = await _api.GetCuponStartDateTimeAsync(e.EventId);
                if (dateTime == null)
                    throw new Exception("알 수 없는 오류: GetCuponStartDateTimeAsync 실패");
                var cupon = new CuponEvent()
                {
                    EventId = e.EventId,
                    EventName = e.EventName,
                    ImageSource = e.ImageSource,
                    Period = e.Period,
                    StartDateTime = dateTime.Value,
                };
                finalList.Add(cupon);
                updateList.Add(cupon);
            }
        }
        if (updateList.Count > 0)
        {
            await _cuponEventRepository.AddCuponEventsAsync(updateList);
        }
        return finalList;
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