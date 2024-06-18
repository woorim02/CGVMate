using CgvMate.Data.Entities.LotteCinema;
using CgvMate.Data.Enums;
using CgvMate.Services.DTOs.LotteCinema;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace CgvMate.Services;

public class LotteService
{
    private readonly HttpClient _client;
    private readonly ILotteGiveawayEventRepository _giveawayEventRepository;

    public LotteService(HttpClient client, ILotteGiveawayEventRepository giveawayEventRepository)
    {
        _client = client;
        _giveawayEventRepository = giveawayEventRepository;
    }

    public async Task<List<Event>> GetEventsAsync(LotteEventType type)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://event.lottecinema.co.kr/LCWS/Event/EventData.aspx");
        var body = new EventsReqDTO(type);
        var json = await SendForm(request, JsonConvert.SerializeObject(body));
        var root = JsonConvert.DeserializeObject<EventsResDTO>(json);

        var kvp = new Dictionary<string, Event>();
        foreach (var item in root.Items)
        {
            kvp.Add(item.EventID, item);
        }
        var views = await _giveawayEventRepository.GetViewsAsync(kvp.Select(x => x.Value.EventID));
        foreach (var view in views)
        {
            kvp[view.Key].Views = view.Value;
        }
        var updateList = new List<Event>();
        foreach (var item in kvp)
        {
            if (!views.Keys.Contains(item.Key))
            {
                updateList.Add(item.Value);
            }
        }
        await _giveawayEventRepository.AddAsync(updateList);
        return kvp.Values.ToList();
    }

    public async Task<LotteGiveawayEventModel> GetLotteGiveawayEventModelAsync(string eventID)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://event.lottecinema.co.kr/LCWS/Event/EventData.aspx");
        var body = new InfomationDeliveryEventDetailReqDTO(eventID);
        var json = await SendForm(request, JsonConvert.SerializeObject(body));
        var root = JsonConvert.DeserializeObject<InfomationDeliveryEventDetailResDTO>(json);
        if (root.IsOK != "true")
            return null;
        await _giveawayEventRepository.UpdateViewAsync(eventID);
        return root.InfomationDeliveryEventDetail[0].GoodsGiftItems[0];
    }

    public async Task<GiveawayEventDetail> GetLotteGiveawayInfoAsync(string eventID, string giftID)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://event.lottecinema.co.kr/LCWS/Event/EventData.aspx");
        var body = new CinemaGoodsReqDTO(eventID, giftID);
        var json = await SendForm(request, JsonConvert.SerializeObject(body));
        var root = JsonConvert.DeserializeObject<CinemaGoodsResDTO>(json);
        var detail = new GiveawayEventDetail()
        {
            CinemaDivisionGoods = root.CinemaDivisionGoods,
            CinemaDivisions = root.CinemaDivisions,
            EventID = eventID,
            GiftID = giftID
        };
        return detail;
    }

    private async Task<string> SendForm(HttpRequestMessage request, string body)
    {
        var form = new MultipartFormDataContent("WebKitFormBoundary");
        var content = new StringContent(body, Encoding.UTF8, "application/json");
        form.Add(content, "paramList");
        request.Content = form;
        var response = await _client.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();
        return json;
    }
}