using CgvMate.Data.Entities.Megabox;

namespace CgvMate.Client.Api;

public class MegaboxMateApi
{
    public MegaboxMateApi(HttpClient client)
    {
        _client = client;
    }

    private readonly HttpClient _client;

    public async Task<List<GiveawayEvent>> GetGiveawayEventListAsync()
    {
        var json = await _client.GetStringAsync($"{Constants.API_HOST}/megabox/event/giveaway/list");
        var events = JsonConvert.DeserializeObject<List<GiveawayEvent>>(json);
        return events;
    }

    public async Task<GiveawayEventDetail> GetGiveawayEventDetailAsync(string goodsNo)
    {
        var json = await _client.GetStringAsync($"{Constants.API_HOST}/megabox/event/giveaway/detail?goodsNo={goodsNo}");
        var detail = JsonConvert.DeserializeObject<GiveawayEventDetail>(json);
        return detail;
    }
}
