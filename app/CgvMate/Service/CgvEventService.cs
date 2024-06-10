using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CgvMate.Service;

public class CgvEventService : CgvServiceBase
{
    private readonly HttpClient _client;

    public CgvEventService(HttpClient client) 
    {
        _client = client;
    }

    public async Task<List<EventInfo>> GetEvents(EventType type)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://m.cgv.co.kr/WebAPP/EventNotiV4/eventMain.aspx/getEventDataList");
        var body = new
        {
            mC = ((int)type).ToString("D3"),
            rC = "GEN",
            tC = "",
            iP = "1",
            pRow = "100",
            rnd6 = "0",
            fList = ""
        };
        request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
        var response = await _client.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        var document = new HtmlDocument();
        document.LoadHtml(content);
        var imageNodes = document.DocumentNode.SelectNodes("//img");
        var periodNodes = document.DocumentNode.SelectNodes("//span[@class='sponsorFpPeriod']");


        var list = new List<EventInfo>();
        for ( var i = 0; i < imageNodes.Count; i++)
        {
            var title = imageNodes[i].Attributes["alt"].Value;
            title = Regex.Replace(title, @"\t|\n|\r", "");
            var imageSrc = imageNodes[i].Attributes["src"].Value;
            var period = periodNodes[i].GetDirectInnerText();
            var code = imageSrc.Replace("https://img.cgv.co.kr/WebApp/contents/eventV4/", "");
            code = code.Split('/')[0];
            var info = new EventInfo(code, title, imageSrc, period);
            list.Add(info);
        }
        return list;
    }

    #region Giveaway
    public async Task<List<GiveawayEvent>> GetGiveawayEventsAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://m.cgv.co.kr/Event/GiveawayEventList.aspx/GetGiveawayEventList");
        request.Content = new StringContent("{theaterCode: '', pageNumber: '1', pageSize: '50'}", Encoding.UTF8, "application/json");
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
            item.GiveawayIndex = giveawayIndex;
        }
        foreach (var item in info.Areas)
        {
            item.IsGiveawayAreaCode = true;
        }
        return info;
    }

    public async Task<bool> SignupGiveawayEvent(GiveawayEventModel model, string theaterCode, string theaterName, string encCount, string ticketNumber, string phoneNumber)
    {
        var payload = new
        {
            eventIdx = Uri.EscapeDataString(Encrypt(model.EventIndex)),
            giveawayIdx = Uri.EscapeDataString(Encrypt(model.GiveawayIndex)),
            giveawayNm = Uri.EscapeDataString(Encrypt(model.Title)),
            ticketNum = Uri.EscapeDataString(Encrypt(ticketNumber)),
            theaterCd = Uri.EscapeDataString(Encrypt(theaterCode)),
            theaterNm = Uri.EscapeDataString(Encrypt(theaterName)),
            mobileNum = Uri.EscapeDataString(Encrypt(phoneNumber)),
            remainCnt = Uri.EscapeDataString(encCount),
            totalCnt = Uri.EscapeDataString(encCount),
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
    #endregion

    #region Cupon
    /// <summary>
    /// 현재 진행중인 스피드쿠폰 이벤트 목록을 불러옵니다. 
    /// </summary>
    /// <returns>리스트. 이벤트 수가 0이면 Empty</returns>
    public async Task<List<SpeedCupon>> GetSpeedCuponCountsAsync()
    {
        var msg = await _client.GetStringAsync($"https://m.cgv.co.kr/Event/2021/fcfs/default.aspx?idx=6");
        var document = new HtmlDocument();
        document.LoadHtml(msg);

        var nameNodes = document.DocumentNode.SelectNodes("//*[@class='btn_reserve']");
        var countNodes = document.DocumentNode.SelectNodes("//*[@class='progress-number']");

        var list = new List<SpeedCupon>();
        for (int i = 0; i < nameNodes.Count; i++)
        {
            string name = nameNodes[i].Attributes["href"].Value;
            var splitValue = name.Split(',');
            string movieIndex = Regex.Replace(splitValue[0], @"\D", "");
            string movieGroupCd = Regex.Replace(splitValue[1], @"\D", "");
            string movienTitle = splitValue[2].Replace("'", "");
            string countStr = countNodes[i].Attributes["aria-valuenow"].Value;
            int count = int.Parse(Regex.Replace(countStr, @"\D", ""));
            list.Add(new SpeedCupon(movienTitle, movieIndex, movieGroupCd, count));
        }
        return list;
    }

    public async Task<SurpriseCupon> GetSurpriseCuponCountAsync(string index)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"https://m.cgv.co.kr/WebApp/EventNotiV4/EventDetailGeneralUnited.aspx?seq={index}");
        var response = await _client.SendAsync(request);
        var msg = await response.Content.ReadAsStringAsync();
        string url = null;
        foreach(var s in msg.Split("\n"))
        {
            if(s.Contains("https://m.cgv.co.kr/Event/2021/fcfs/default.aspx"))
            {
                url = Regex.Replace(s.Trim(), @"\t|\n|\r", "").Replace("window.location.href = \"", "").Replace("\";", "");
                break;
            }
        }
        var document = new HtmlDocument();
        document.LoadHtml(await _client.GetStringAsync(url));

        var titleAttr = document.DocumentNode.SelectSingleNode("//*[@property='og:title']").Attributes["content"];
        var titleStr = titleAttr.Value.ToString();

        var countAttr = document.DocumentNode.SelectSingleNode("//*[@class='progress-number']").Attributes["aria-valuenow"];
        var countStr = countAttr.Value.ToString().Trim().Replace(",", "").Replace("쿠폰 사용 수량", "");
        var count = int.Parse(countStr);

        var avaAttr = document.DocumentNode.SelectSingleNode("//*[@class='btn_reserve']").Attributes["href"];
        bool ava = !avaAttr.Value.ToString().Contains("소진되었습니다");

        return new SurpriseCupon(index, titleStr, count, ava);
    }
    #endregion
}

public enum EventType
{
    Special = 001,
    Movie = 004,
    Theater = 005,
    Affiliate = 006,
    Membership_Club = 008,
    Past = 100
}
