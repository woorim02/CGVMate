using System.Text.RegularExpressions;

namespace CgvMate.Client;

public class CgvMateApi
{
    HttpClient _client;
    public CgvMateApi(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<EventInfo>> GetEventListAsync(EventType type)
    {
        var json = await _client.GetStringAsync($"https://{Constants.API_HOST}/cgv/event/list?type={(int)type}");
        List<EventInfo> infos = JsonConvert.DeserializeObject<List<EventInfo>>(json);
        return infos;
    }

    public async Task<List<string>> GetEventImgSrcAsync(string eventId){
        var json = await _client.GetStringAsync($"https://{Constants.API_HOST}/cgv/event/detail/{eventId}");
        var list = JsonConvert.DeserializeObject<List<string>>(json);
        return list;
    }

    public async Task<List<GiveawayEvent>> GetGiveawayEventListAsync()
    {
        var json = await _client.GetStringAsync($"https://{Constants.API_HOST}/cgv/event/giveaway/list");
        var events = JsonConvert.DeserializeObject<List<GiveawayEvent>>(json);
        return events;
    }

    public async Task<GiveawayEventModel> GetGiveawayEventModelAsync(string eventIndex)
    {
        var json = await _client.GetStringAsync($"https://{Constants.API_HOST}/cgv/event/giveaway/model?eventIndex={eventIndex}");
        var model = JsonConvert.DeserializeObject<GiveawayEventModel>(json);
        return model;
    }

    public async Task<GiveawayInfo> GetGiveawayInfoAsync(string giveawayIndex, string areaCode = "")
    {
        var json = await _client.GetStringAsync($"https://{Constants.API_HOST}/cgv/event/giveaway/info?giveawayIndex={giveawayIndex}&areaCode={areaCode}");
        var info = JsonConvert.DeserializeObject<GiveawayInfo>(json);
        return info;
    }
    #region Cupon
    /// <summary>
    /// 현재 진행중인 스피드쿠폰 이벤트 목록을 불러옵니다. 
    /// </summary>
    /// <returns>리스트. 이벤트 수가 0이면 Empty</returns>
    public async Task<List<SpeedCupon>> GetSpeedCuponCountsAsync()
    {
        var msg = await _client.GetStringAsync($"https://{Constants.API_HOST}/proxy/Event/2021/fcfs/default.aspx?idx=6");
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
        var request = new HttpRequestMessage(HttpMethod.Get, $"https://{Constants.API_HOST}/proxy/WebApp/EventNotiV4/EventDetailGeneralUnited.aspx?seq={index}");
        var response = await _client.SendAsync(request);
        var msg = await response.Content.ReadAsStringAsync();
        string url = null;
        foreach (var s in msg.Split("\n"))
        {
            if (s.Contains("https://m.cgv.co.kr/Event/2021/fcfs/default.aspx"))
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
