using CgvMate.Data.Entities.Cgv;
using CgvMate.Services.DTOs.Cgv;
using HtmlAgilityPack;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text;
using System.Text.RegularExpressions;
using Tesseract;

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
        var msg = await _client.GetStringAsync($"https://m.cgv.co.kr/WebApp/EventNotiV4/EventDetailGeneralUnited.aspx?seq={cuponId}");
        string url = null;
        foreach (var s in msg.Split("\n"))
        {
            if (s.Contains("https://m.cgv.co.kr/Event/2021/fcfs/default.aspx"))
            {
                url = Regex.Replace(s.Trim(), @"\t|\n|\r", "").Replace("window.location.href = \"", "").Replace("\";", "");
                break;
            }
        }
        var html = await _client.GetStringAsync(url);

        var document = new HtmlDocument();
        string imageUrl;
        try
        {
            imageUrl = document.DocumentNode
                .SelectSingleNode("//div[contains(@class, 'swiper-slide') and contains(@class, 'ver1') and contains(@class, 'swiper-slide-active')]/div/img")
                .Attributes["src"]
                .Value;
        }
        catch (Exception ex)
        {
            throw new Exception("쿠폰 이미지 url 파싱 실패", ex);
        }
        
        var text = await OCRImageAsync(imageUrl);
        var dateTime = ExtractDateTime(text);
        return dateTime;
    }

    public async Task<List<GiveawayEvent>> GetGiveawayEventsAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://m.cgv.co.kr/Event/GiveawayEventList.aspx/GetGiveawayEventList");
        request.Content = new StringContent("{theaterCode: '', pageNumber: '1', pageSize: '50'}", Encoding.UTF8, "application/json");
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

    private async Task<string> OCRImageAsync(string imageUrl)
    {
        // Tesseract 엔진 초기화
        using var engine = new TesseractEngine(@"./tessdata", "kor", EngineMode.Default);
        // 이미지 파일로부터 텍스트 추출
        var bytes = await _client.GetByteArrayAsync(imageUrl);
        using var img = Pix.LoadFromMemory(bytes);
        using var page = engine.Process(img);
        // 추출된 텍스트 출력
        string text = page.GetText();
        return text;
    }

    private static DateTime? ExtractDateTime(string input)
    {
        // 날짜 패턴: 숫자 / 숫자 ( 예: 7/8 )
        string datePattern = @"\b\d{1,2}/\d{1,2}\b";
        // 시간 패턴: 숫자:숫자 ( 예: 15:00 )
        string timePattern = @"\b\d{1,2}:\d{2}\b";

        var dateMatch = Regex.Match(input, datePattern);
        var timeMatch = Regex.Match(input, timePattern);

        if (dateMatch.Success && timeMatch.Success)
        {
            try
            {
                // 날짜 문자열을 분리 ( 예: 7/8 -> 7월 8일 )
                var dateParts = dateMatch.Value.Split('/');
                int month = int.Parse(dateParts[0]);
                int day = int.Parse(dateParts[1]);

                // 시간 문자열을 분리 ( 예: 15:00 -> 15시 00분 )
                var timeParts = timeMatch.Value.Split(':');
                int hour = int.Parse(timeParts[0]);
                int minute = int.Parse(timeParts[1]);

                // 현재 연도 가져오기
                int year = DateTime.Now.Year;

                // DateTime 객체로 반환
                return new DateTime(year, month, day, hour, minute, 0);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error parsing date and time: {e.Message}");
            }
        }

        return null; // 유효한 날짜와 시간을 찾지 못했을 경우
    }
}
