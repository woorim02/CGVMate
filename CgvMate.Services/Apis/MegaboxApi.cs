using CgvMate.Data.Entities.Megabox;
using CgvMate.Services.DTOs.Megabox;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace CgvMate.Services.Apis;

internal class MegaboxApi
{
    public MegaboxApi(HttpClient httpClient)
    {
        _client = httpClient;
    }

    HttpClient _client;

    public async Task<List<Event>> GetCuponEventsAsync()
    {
        var req = new HttpRequestMessage(HttpMethod.Post, "https://m.megabox.co.kr/on/oh/ohe/Event/eventMngDiv.do");
        var body = new EventReqDTO()
        {
            currentPage = "1",
            eventDivCd = "CED01",
            eventStatCd = "ONG",
            eventTitle = "빵원",
            eventTyCd = "",
            orderReqCd = "ONGlist",
            recordCountPerPage = "100",
            totCnt = 1
        };
        req.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
        var res = await _client.SendAsync(req);
        var html = await res.Content.ReadAsStringAsync();
        var events = ParseEvents(html);
        return events;
    }

    public async Task<GiveawayEventDetail?> GetGiveawayEventDetailAsync(string goodsNo)
    {
        var html = await _client.GetStringAsync($"https://www.megabox.co.kr/on/oh/ohe/Event/selectGoodsStockPrco.do?goodsNo={goodsNo}");
        var document = new HtmlDocument();
        document.LoadHtml(html);

        var detail = new GiveawayEventDetail();
        detail.ID = goodsNo;
        detail.Title = document.DocumentNode.SelectSingleNode("//div[@class='tit']").InnerText;
        if (string.IsNullOrEmpty(detail.Title))
        {
            return null;
        }
        detail.Areas = new List<AreaGiveawayInfo>();
        var areaNodes = document.DocumentNode.SelectNodes("//li[contains(@class, 'area-cont')]");
        if (areaNodes == null)
            return null;

        foreach (var areaNode in areaNodes)
        {
            var areaGiveawayInfo = new AreaGiveawayInfo();
            areaGiveawayInfo.Code = areaNode.Attributes["id"].Value;
            areaGiveawayInfo.Name = areaNode.SelectSingleNode(".//button[@class='btn']").InnerText;
            areaGiveawayInfo.Infos = new List<TheaterGiveawayInfo>();

            var theaterNodes = areaNode.SelectNodes(".//li[@class='brch']");
            foreach (var theaterNode in theaterNodes)
            {
                var theaterGiveawayInfo = new TheaterGiveawayInfo();
                theaterGiveawayInfo.ID = theaterNode.Attributes["id"].Value;
                theaterGiveawayInfo.Name = theaterNode.SelectSingleNode(".//a").InnerText;
                theaterGiveawayInfo.fAc = theaterNode.SelectSingleNode(".//span").InnerText;
                areaGiveawayInfo.Infos.Add(theaterGiveawayInfo);
            }
            detail.Areas.Add(areaGiveawayInfo);
        }
        return detail;
    }

    private static List<Event> ParseEvents(string html)
    {
        var events = new List<Event>();
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var eventNodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='event-list v100']/div[@class='item']");

        foreach (var eventNode in eventNodes)
        {
            var titleNode = eventNode.SelectSingleNode(".//p[@class='title']");
            var dateNode = eventNode.SelectSingleNode(".//p[@class='date']");
            var imgNode = eventNode.SelectSingleNode(".//img");
            var linkNode = eventNode.SelectSingleNode(".//a");

            var evt = new Event
            {
                Title = titleNode?.InnerText.Trim(),
                Date = dateNode?.InnerText.Trim(),
                ImageUrl = imgNode?.GetAttributeValue("data-src", ""),
                EventNo = linkNode?.GetAttributeValue("onclick", "").Replace("javascript:fn_eventDetail(", "").Replace(");", "").Trim()
            };
            var matches = Regex.Matches(evt.EventNo, @"\d+");
            evt.EventNo = "";
            foreach (Match match in matches)
            {
                evt.EventNo += match.Value;
            }

            events.Add(evt);
        }

        return events;
    }
}