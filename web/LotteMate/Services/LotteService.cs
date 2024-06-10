using System.Formats.Asn1;
using System.Text;
using Newtonsoft.Json;
namespace LotteMate;

public partial class LotteService
{
    HttpClient _client;
    public LotteService(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<LotteEvent>> GetEventsAsync(LotteEventType type)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://event.lottecinema.co.kr/LCWS/Event/EventData.aspx");
        var body = new LotteEventsRequestBody(type);
        var json = await SendForm(request, JsonConvert.SerializeObject(body));
        var root = JsonConvert.DeserializeObject<LotteEventRoot>(json);
        return root.Items;
    }

    public async Task<LotteGiveawayEventModel> GetLotteGiveawayEventModelAsync(string eventID)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://event.lottecinema.co.kr/LCWS/Event/EventData.aspx");
        var body = new InfomationDeliveryEventDetailRequest(eventID);
        var json = await SendForm(request, JsonConvert.SerializeObject(body));
        var root = JsonConvert.DeserializeObject<InfomationDeliveryEventDetailResponse>(json);
        if (root.IsOK != "true")
            return null;
        return root.InfomationDeliveryEventDetail[0].GoodsGiftItems[0];
    }

    public async Task<LotteGiveawayInfo> GetLotteGiveawayInfoAsync(string eventID, string giftID)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://event.lottecinema.co.kr/LCWS/Event/EventData.aspx");
        var body = new CinemaGoodsRequestBody(eventID, giftID);
        var json = await SendForm(request, JsonConvert.SerializeObject(body));
        var root = JsonConvert.DeserializeObject<LotteGiveawayInfo>(json);
        return root;
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
