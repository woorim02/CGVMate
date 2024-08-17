using CgvMate.Data.Entities.Cgv;
using CgvMate.Services.DTOs.Cgv;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace CgvMate.Services.Apis;

internal class CgvApi
{
    public CgvApi(HttpClient client, Func<string,string> decrypt)
    {
        _client = client;
        Decrypt = decrypt;
    }

    private readonly HttpClient _client;
    private readonly Func<string, string> Decrypt;

    public async Task<List<Event>> GetEvents(CgvEventType type)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://m.cgv.co.kr/WebAPP/EventNotiV4/eventMain.aspx/getEventDataList");
        var body = new EventReqDTO(type);
        request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

        var response = await _client.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();

        var document = new HtmlDocument();
        document.LoadHtml(content);
        var imageNodes = document.DocumentNode.SelectNodes("//img");
        var periodNodes = document.DocumentNode.SelectNodes("//span[@class='sponsorFpPeriod']");

        var list = new List<Event>();
        for (var i = 0; i < imageNodes.Count; i++)
        {
            var imageSrc = imageNodes[i].Attributes["src"].Value;
            var info = new Event()
            {
                EventId = imageSrc.Replace("https://img.cgv.co.kr/WebApp/contents/eventV4/", "").Split('/')[0],
                EventName = Regex.Replace(imageNodes[i].Attributes["alt"].Value, @"\t|\n|\r", ""),
                ImageSource = imageSrc,
                Period = periodNodes[i].GetDirectInnerText(),
            };
            list.Add(info);
        }
        return list;
    }

    public async Task<DateTime?> GetCuponStartDateTimeAsync(string cuponId)
    {
        var req = new HttpRequestMessage(HttpMethod.Get, $"http://www.cgv.co.kr/culture-event/event/detailViewUnited.aspx?seq={cuponId}");
        req.Headers.Add("Host", "www.cgv.co.kr");
        var res = await _client.SendAsync(req);
        var html = await res.Content.ReadAsStringAsync();
        string dateString = null;

        foreach (var s in html.Split("\n"))
        {
            if (s.Contains("var startDate = "))
            {
                dateString = ExtractBracketContent(s);
                break;
            }
        }

        if (dateString == null)
        {
            throw new Exception("날짜 파싱 실패");
        }

        var dateTime = DateTime.ParseExact(dateString, "yyyyMMddHHmm", CultureInfo.InvariantCulture);
        return dateTime;
    }

    public async Task<List<GiveawayEvent>> GetGiveawayEventsAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://m.cgv.co.kr/Event/GiveawayEventList.aspx/GetGiveawayEventList");
        request.Content = new StringContent("{theaterCode: '', pageNumber: '1', pageSize: '100'}", Encoding.UTF8, "application/json");
        var response = await _client.SendAsync(request);

        var content = await response.Content.ReadAsStringAsync();
        var obj = JsonConvert.DeserializeObject<GiveawayEventListResDTO>(content);
        var document = new HtmlDocument();

        try
        {
            var htmlText = obj.d.List.Replace(" onclick='detailEvent(this, \"False\")'", "");
            document.LoadHtml(htmlText);
        }
        catch (Exception e) { throw new InvalidDataException(content, e); }

        var list = new List<GiveawayEvent>();
        try
        {
            foreach (var i in document.DocumentNode.ChildNodes)
            {
                GiveawayEvent giveawayEvent = new GiveawayEvent()
                {
                    Title = i.SelectSingleNode("div/strong[1]").InnerText,
                    Period = i.SelectSingleNode("div/span[1]").InnerText,
                    EventIndex = i.Attributes["data-eventIdx"].Value,
                    DDay = i.SelectSingleNode("div/span[2]").InnerText
                };
                list.Add(giveawayEvent);
            }
        }
        catch (Exception e) { throw new InvalidDataException(content, e); }
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
        var model = obj["d"]?.ToObject<GiveawayEventModelResDTO>();
        if (model == null) { throw new InvalidDataException(content); }

        return new GiveawayEventModel()
        {
            GiveawayIndex = model.GiveawayIndex,
            EventIndex = model.EventIndex,
            Contents = model.Contents,
            Title = model.Title,
        };
    }

    public async Task<GiveawayEventDetail> GetGiveawayInfoAsync(string giveawayIndex, string areaCode = "")
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://m.cgv.co.kr/Event/GiveawayEventDetail.aspx/GetGiveawayTheaterInfo");
        request.Content = new StringContent($"{{giveawayIndex: '{giveawayIndex}', areaCode: '{areaCode}'}}", Encoding.UTF8, "application/json");
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        string content = await response.Content.ReadAsStringAsync();
        var obj = JObject.Parse(content);
        var info = obj["d"]?.ToObject<GiveawayEventDetail>();
        if (info == null) { throw new InvalidDataException(content); }

        foreach (var item in info.TheaterGiveawayInfos)
        {
            item.GiveawayRemainCount = Decrypt(item.EncCount);
            item.GiveawayIndex = giveawayIndex;
        }
        foreach (var item in info.Areas)
        {
            item.IsGiveawayAreaCode = true;
        }
        return info;
    }

    public static string ExtractBracketContent(string input)
    {
        var match = Regex.Match(input, @"\'(.*?)\'");
        return match.Success ? match.Groups[1].Value : string.Empty;
    }
}
