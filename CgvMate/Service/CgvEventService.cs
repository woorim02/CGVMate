using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace CgvMate.Service;

public class CgvEventService : CgvServiceBase
{
    private readonly HttpClient _client;

    public CgvEventService(HttpClient client) 
    {
        _client = client;
    }

    #region Get
    public async Task<List<GiveawayEvent>> GetGiveawayEventsAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://m.cgv.co.kr/Event/GiveawayEventList.aspx/GetGiveawayEventList");
        request.Content = new StringContent("{theaterCode: '', pageNumber: '1', pageSize: '30'}", Encoding.UTF8, "application/json");
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var obj = JObject.Parse(content);
        var document = new HtmlDocument();

        try {
            var htmlText = obj["d"]["List"].ToString().Replace(" onclick='detailEvent(this, \"False\")'", "");
            document.LoadHtml(htmlText); 
        }
        catch (Exception e){ throw new InvalidDataException(content, e); }

        var list = new List<GiveawayEvent>();
        try
        {
            foreach (var i in document.DocumentNode.ChildNodes)
            {
				GiveawayEvent giveawayEvent = new GiveawayEvent(
                    title: i.SelectSingleNode("div/strong[1]").InnerText,
                    period: i.SelectSingleNode("div/span[1]").InnerText,
                    eventIndex: i.Attributes["data-eventIdx"].Value,
					dDay: i.SelectSingleNode("div/span[2]").InnerText);
                list.Add(giveawayEvent);
            }
        }
        catch(Exception e) { throw new InvalidDataException(content, e); }
        return list;
    }

    public async Task<GiveawayEventModel> GetGiveawayEventModelAsync(string eventIndex)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://m.cgv.co.kr/Event/GiveawayEventDetail.aspx/GetGiveawayEventDetail");
        request.Content = new StringContent($"{{eventIndex: '{eventIndex}', giveawayIndex: ''}}", Encoding.UTF8, "application/json");
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var obj = JObject.Parse(content);
        var model = obj["d"]?.ToObject<GiveawayEventModel>();
        if(model == null) { throw new InvalidDataException(content); }

        return model;
    }

    public async Task<GiveawayInfo> GetGiveawayInfoAsync(string giveawayIndex, string areaCode = "")
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://m.cgv.co.kr/Event/GiveawayEventDetail.aspx/GetGiveawayTheaterInfo");
        request.Content = new StringContent($"{{giveawayIndex: '{giveawayIndex}', areaCode: '{areaCode}'}}", Encoding.UTF8, "application/json");
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        string content = await response.Content.ReadAsStringAsync();
        var obj = JObject.Parse(content);
        var info = obj["d"]?.ToObject<GiveawayInfo>();
        if (info == null) { throw new InvalidDataException(content); }

        foreach (var item in info.TheaterGiveawayInfos)
        {
            item.GiveawayRemainCount = Decrypt(item.EncCount);
        }
        foreach (var item in info.Areas)
        {
            item.IsGiveawayAreaCode = true;
        }
        return info;
    }
    #endregion

    public async Task<bool> SignupGiveawayEvent(GiveawayEventModel model, TheaterGiveawayInfo info, string ticketNumber, string phoneNumber)
    {
        var payload = new
        {
            eventIdx = Uri.EscapeDataString(Encrypt(model.EventIndex)),
            giveawayIdx = Uri.EscapeDataString(Encrypt(model.GiveawayIndex)),
            giveawayNm = Uri.EscapeDataString(Encrypt(model.Title)),
            ticketNum = Uri.EscapeDataString(Encrypt(ticketNumber)),
            theaterCd = Uri.EscapeDataString(Encrypt(info.TheaterCode)),
            theaterNm = Uri.EscapeDataString(Encrypt(info.TheaterName)),
            mobileNum = Uri.EscapeDataString(Encrypt(phoneNumber)),
            remainCnt = Uri.EscapeDataString(info.EncCount),
            totalCnt = Uri.EscapeDataString(info.EncCount),
        };
        var request = new HttpRequestMessage(HttpMethod.Post, "https://m.cgv.co.kr/Event/GiveawayEventSignup.aspx/SignGiveawayNum");
        request.Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
        var response = await _client.SendAsync(request);
        var obj = JObject.Parse(await response.Content.ReadAsStringAsync());
        var resultCd = obj["d"][0].ToString();
        if (resultCd == "00")
            return true;
        return false;
    }
}
