using CgvMate.Data.Entities.Megabox;
using HtmlAgilityPack;

namespace CgvMate.Services;

internal class MegaboxApi
{
    public MegaboxApi(HttpClient httpClient)
    {
        _client = httpClient;
    }

    HttpClient _client;

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
        if(areaNodes == null)
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
}