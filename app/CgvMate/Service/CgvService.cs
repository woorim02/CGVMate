using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace CgvMate.Service;

public class CgvService
{
    private HttpClient _client;
    private SocketsHttpHandler _handler;
    private HttpClient _authClient;
    private SocketsHttpHandler _authHandler;
    
    public CgvEventService Event { get; private set; }
    public CgvAuthService Auth { get; private set; }
    public CgvReservationService Reservation { get; private set; }

    public CgvService(HttpClient client, HttpClient authClient, SocketsHttpHandler handler, SocketsHttpHandler authHandler)
    {
        _client = client;
        _authClient = authClient;
        _handler = handler;
        _authHandler = authHandler;
        Event = new CgvEventService(_client);
        Auth = new CgvAuthService(_authClient, _authHandler);
        Reservation = new CgvReservationService(_client, _handler, _authClient, _authHandler);
    }

    public async Task<List<Area>> GetAreasAsync()
    {
        var response = await _client.GetAsync("https://m.cgv.co.kr/WebApp/TheaterV4/");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var document = new HtmlDocument();
        document.LoadHtml(content);

        var areaCgvNode = document.DocumentNode.SelectSingleNode("//div[@class='cgv_choice linktype area']");
        var areasNode = areaCgvNode.SelectNodes("ul/li");
        var areas = new List<Area>();
        for (int i = 0; i < areasNode.Count; i++)
        {
            try
            {
                var regioncode = areasNode[i].SelectSingleNode("a").Attributes["href"].Value.Split('=')[1];
                var innerText = areasNode[i].SelectSingleNode("a/div/strong").InnerText.Replace(")", string.Empty);
                var name = innerText.Split("(")[0];
                var count = innerText.Split("(")[1];
                name = Regex.Replace(name, @"\s+", string.Empty);
                count = Regex.Replace(count, @"\s+", string.Empty);
                areas.Add(new Area(name, regioncode, false, count));
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(content, ex);
            }
        }
        return areas;
    }

    public async Task<List<Theater>> GetTheatersAsync(string regionCode)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://m.cgv.co.kr/WebApp/MyCgvV5/favoriteTheater.aspx/GetRegionTheaterList");
        request.Content = new StringContent($"{{ regionCode: '{regionCode}'}}", Encoding.UTF8, "application/json");
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        List<Theater> theaters;
        try
        {
            var obj = JObject.Parse(content);
            var arr = JArray.Parse(obj["d"].ToString());
            theaters = arr.ToObject<List<Theater>>();
        }
        catch (Exception ex)
        {
            throw new InvalidDataException(content, ex);
        }
        return theaters;
    }

    public async Task<List<Movie>> GetMoviesAsync()
    {
        var movies = new List<Movie>();
        var firstRequest = new HttpRequestMessage(HttpMethod.Get, "https://m.cgv.co.kr/WebAPP/MovieV4/movieList.aspx?iPage=1");
        var firstResponse = await _client.SendAsync(firstRequest);
        firstResponse.EnsureSuccessStatusCode();
        var firstContent = await firstResponse.Content.ReadAsStringAsync();
        var firstList = ParseMovieList(firstContent);
        movies.AddRange(firstList);
        if (firstList.Count < 20)
            return movies;
        for (int i = 2; true; i++)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://m.cgv.co.kr/WebAPP/MovieV4/ajaxMovie.aspx");
            var payload = new Dictionary<string, string>
            {
                { "iPage" , $"{i}"},
                { "pageRow" , $"{20}"},
                { "mtype", "now" },
                { "morder", "TicketRate" },
                { "mnowflag",  $"{0}" },
                { "mdistype", "" },
                { "flag", "MLIST" }
            };
            request.Content = new FormUrlEncodedContent(payload);
            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            if (content == "")
                break;
            var list = ParseMovieList(content);
            movies.AddRange(list);
            if (list.Count < 20)
                break;
        }

        return movies;
    }

    public async Task<List<Movie>> SearchMoviesAsync(string keyword)
    {
        var checkRequest = new HttpRequestMessage(HttpMethod.Post, "https://m.cgv.co.kr/WebAPP/Search/Default.aspx/CheckKeyword");
        checkRequest.Content = new StringContent($"{{keyword: '{HttpUtility.UrlEncode(keyword)}'}}", Encoding.UTF8, "application/json");
        var checkResponse = await _client.SendAsync(checkRequest);
        checkResponse.EnsureSuccessStatusCode();
        var checkResponseObject = JObject.Parse(await checkResponse.Content.ReadAsStringAsync());
        if (checkResponseObject["d"]?.ToString() != "00000")
            return null;

        var getMovieListRequest = new HttpRequestMessage(HttpMethod.Post, "https://m.cgv.co.kr/WebAPP/Search/Default.aspx/GetMovieInfoList");
        getMovieListRequest.Content = new StringContent($"{{ pageIndex: '1', pageSize:'30', keyword: '{HttpUtility.UrlEncode(keyword)}'}}", Encoding.UTF8, "application/json");
        var getMovieListResponse = await _client.SendAsync(getMovieListRequest);
        getMovieListResponse.EnsureSuccessStatusCode();
        var obj = JObject.Parse(await getMovieListResponse.Content.ReadAsStringAsync());

        string? countValue = obj?["d"]?["Count"]?.ToString();
        if (string.IsNullOrEmpty(countValue) || countValue == "0")
            return null;

        var document = new HtmlDocument();
        document.LoadHtml(obj["d"]["Contents"].ToString());
        var nodes = document.DocumentNode.SelectNodes("//img");
        var movies = new List<Movie>();
        for (int i = 0; i < nodes.Count; i++)
        {
            var title = nodes[i].Attributes["alt"].Value;
            var imgSource = nodes[i].Attributes["src"].Value;
            var sp = imgSource.Split('/');
            var index = imgSource.Split('/')[sp.Length - 1].Split('_')[0];
            var movie = new Movie()
            {
                Title = title,
                ThumbnailSource = imgSource,
                Index = index,

            };
            movies.Add(movie);
        }
        return movies;
    }

    public async Task<string> GetMovieGroupCdAsync(string movieIndex)
    {
        var htmlText = await GetFanpageHtmlText(movieIndex);
        var reader = new StringReader(htmlText);
        while (true)
        {
            string? line = reader.ReadLine();
            if (line == null)
                throw new InvalidDataException("무비그룹을 찾을 수 없음");
            if (line.Contains("mgCD"))
            {
                var value = Regex.Replace(line, @"\D", string.Empty);
                return value;
            }
        }
    }

    public async Task<string[]> GetScreenTypesAsync(string movieIndex)
    {
        var list = new List<string> { "2D" };
        var document = new HtmlDocument();
        var text = await GetFanpageHtmlText(movieIndex);
        if (text.Contains("페이지를 찾을 수 없습니다."))
            throw new InvalidDataException("페이지를 찾을 수 없습니다.");
        document.LoadHtml(text);
        var nodes = document.DocumentNode.SelectNodes("//ul[@class='screenType']/li/img");
        if (nodes == null)
            return list.ToArray();
        foreach (var n in nodes)
            list.Add(n.Attributes["alt"].Value);
        return list.ToArray();
    }

    private async Task<string> GetFanpageHtmlText(string movieIndex)
    {
        var gateWayResponse = await _client.GetAsync($"https://m.cgv.co.kr/WebApp/fanpage/Gateway.aspx?movieIdx={movieIndex}");
        var request = new HttpRequestMessage(HttpMethod.Post, $"https://moviestory.cgv.co.kr/fanpage/login");
        request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0;) Chrome/120.0.0.0 Safari/537.36");
        request.Headers.Host = "moviestory.cgv.co.kr";
        request.Headers.Add("Cookie", gateWayResponse.Headers.GetValues("Set-Cookie"));
        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            {"fanpageMovieIdx", movieIndex },
            {"fanpageIsWebView","false" }
        });
        //request.Headers.Host = "moviestory.cgv.co.kr";
        var response = await _client.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        return content;
    }

    private static List<Movie> ParseMovieList(string content)
    {
        List<Movie> movies = new List<Movie>();
        var document = new HtmlDocument();
        document.LoadHtml(content);
        var imageNodes = document.DocumentNode.SelectNodes("//span[@class='imgbox']/img");
        var contentsNodes = document.DocumentNode.SelectNodes("//div[@class='txtbox']");
        var jsNodes = document.DocumentNode.SelectNodes("//a[@class='btn_reserve']");
        var icoTheaterNodes = document.DocumentNode.SelectNodes("//div[@class='ico_theater2']");
        if ((imageNodes.Count != contentsNodes.Count) || (contentsNodes.Count != jsNodes.Count) || (jsNodes.Count != icoTheaterNodes.Count))
            throw new InvalidDataException($"Count err! {imageNodes.Count}, {contentsNodes.Count}, {jsNodes.Count}, {icoTheaterNodes.Count}");
        if (imageNodes.Count == 0)
            return movies;
        for (int i = 0; i < contentsNodes.Count; i++)
        {
            try
            {
                string title = imageNodes[i].Attributes["alt"].Value;
                string imgSource = imageNodes[i].Attributes["src"].Value;

                var spArr = imgSource.Split("/");
                string index = imgSource.Split('/')[spArr.Length - 1].Split('_')[0];
                string movieGroupCd = jsNodes[i].Attributes["onclick"].Value.Split("', '")[1];

                var spanNodes = icoTheaterNodes[i].SelectNodes("span");
                List<string> screenTypes = ["2D"];
                var list = spanNodes?.Select(s => s.InnerText);
                if (list != null)
                    screenTypes.AddRange(list);

                var movie = new Movie()
                {
                    Title = title,
                    Index = index,
                    ThumbnailSource = imgSource,
                    MovieGroupCd = movieGroupCd,
                    ScreenTypes = screenTypes.ToArray()
                };
                movies.Add(movie);
            }
            catch (Exception ex)
            {
                throw new InvalidDataException("MovieList Parse err!", ex);
            }
        }
        return movies;
    }
}
