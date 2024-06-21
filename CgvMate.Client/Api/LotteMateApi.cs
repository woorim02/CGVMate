using CgvMate.Data.Entities.LotteCinema;
using CgvMate.Data.Enums;
using Newtonsoft.Json.Linq;

namespace CgvMate.Client;

public class LotteMateApi
{
    HttpClient _client;
    public LotteMateApi(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<Event>> GetLotteEventListAsync(LotteEventType type)
    {
        var json = await _client.GetStringAsync($"{Constants.API_HOST}/lotte/event/list?type={(int)type}");
        var infos = JsonConvert.DeserializeObject<List<Event>>(json);
        return infos;
    }

    public async Task<List<Event>> GetLotteGiveawayEventListAsync()
    {
        var json = await _client.GetStringAsync($"{Constants.API_HOST}/lotte/event/giveaway/list");
        var infos = JsonConvert.DeserializeObject<List<Event>>(json);
        return infos;
    }

    public async Task<LotteGiveawayEventModel> GetLotteGiveawayEventModelAsync(string eventID)
    {
        var json = await _client.GetStringAsync($"{Constants.API_HOST}/lotte/event/giveaway/model?event_id={eventID}");
        var obj = JsonConvert.DeserializeObject<LotteGiveawayEventModel>(json);
        return obj;
    }

    public async Task<GiveawayEventDetail> GetLotteGiveawayInfoAsync(string event_id, string gift_id){
        var json = await _client.GetStringAsync($"{Constants.API_HOST}/lotte/event/giveaway/info?event_id={event_id}&gift_id={gift_id}");
        var info = JsonConvert.DeserializeObject<GiveawayEventDetail>(json);
        return info;
    }
}
