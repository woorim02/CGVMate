using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace CgvMate.Service;

public class CgvReservationService : CgvServiceBase
{
    private readonly HttpClient _client;
    private readonly SocketsHttpHandler _handler;
    private readonly HttpClient _authClient;
    private readonly SocketsHttpHandler _authHandler;

    public CgvReservationService(HttpClient client, SocketsHttpHandler handler, HttpClient authClient, SocketsHttpHandler authHandler)
    {
        _client = client;
        _handler = handler;
        _authClient = authClient;
        _authHandler = authHandler;
    }

    public async Task<TheaterScheduleListRoot> GetScheduleListAsync(string theaterCode, string? movieGroupCd, DateTime date, string screenTypeCode = "02")
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "https://m.cgv.co.kr/WebApp/Reservation/Schedule.aspx");
        await _client.SendAsync(request);

        request = new HttpRequestMessage(HttpMethod.Post, "https://m.cgv.co.kr/WebAPP/Reservation/Common/ajaxTheaterScheduleList.aspx/GetTheaterScheduleList");
        request.Headers.Add("Accept", "application/json, text/javascript, */*; q=0.01");
        request.Headers.Add("Accept-Language", "ko-KR,ko;q=0.8,en-US;q=0.5,en;q=0.3");
        request.Headers.Add("Origin", "https://m.cgv.co.kr");
        request.Headers.Add("Cookie", "URL_PREV_COMMON=");
        var body = new
        {
            strRequestType = "THEATER",
            strUserID = "",
            strMovieGroupCd = movieGroupCd,
            strMovieTypeCd = "",
            strPlayYMD = $"{date:yyyyMMdd}",
            strTheaterCd = theaterCode,
            strScreenTypeCd = screenTypeCode,
            strRankType = "MOVIE",
        };
        request.Content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

        var response = await _client.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        content = JObject.Parse(content)["d"]?.ToString();
        var root = JsonConvert.DeserializeObject<TheaterScheduleListRoot>(content);
        return root;
    }

    public async Task<MiniMapDataRoot> GetMiniMapDataAsync(Schedule schedule)
    {
        var json = new
        {
            movieAttrCd = schedule.MovieAttrCd,
            movieAttrNm = schedule.MovieAttrNm,
            movieCd = schedule.MovieCd,
            movieGroupCd = schedule.MovieGroupCd,
            movieRatingCd = schedule.MovieRatingCd,
            playNum = schedule.PlayNum,
            playStartTm = schedule.PlayStartTm,
            playYMD = schedule.PlayYmd,
            requestType = "01",
            screenCd = schedule.ScreenCd,
            screenRatingCd = schedule.ScreenRatingCd,
            seatRemainRate = schedule.SeatRate,
            theaterCd = schedule.TheaterCd,
            userId = ""
        };
        var request = new HttpRequestMessage(HttpMethod.Post, "https://m.cgv.co.kr/WebAPP/Reservation/Common/ajaxMovieMiniMapData.aspx/GetMiniMapData");
        request.Content = new StringContent(JsonConvert.SerializeObject(json), Encoding.UTF8, "application/json");
        var response = await _client.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        content = JObject.Parse(content)["d"].ToString();
        var root = JsonConvert.DeserializeObject<MiniMapDataRoot>(content);
        return root;
    }
}
