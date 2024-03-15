using System.Net;

namespace CgvMate.Service;

public class CgvAuthService : CgvServiceBase
{
    private readonly HttpClient _authClient;
    private readonly SocketsHttpHandler _authHandler;

    public CgvAuthService(HttpClient authClient, SocketsHttpHandler authHandler)
    {
        _authClient = authClient;
        _authHandler = authHandler;
    }

    public async Task<bool> LoginAsync(string id, string password)
    {
        var getCookieResponse = await _authClient.GetAsync("https://m.cgv.co.kr/Webapp/Member/Login.aspx");
        getCookieResponse.EnsureSuccessStatusCode();
        var checkIpResponse = await _authClient.GetAsync("https://m.cgv.co.kr/WebAPP/Member/Login.aspx/CheckIP");
        checkIpResponse.EnsureSuccessStatusCode();
        var captchaResponse = await _authClient.GetAsync("https://m.cgv.co.kr/WebAPP/Member/Login.aspx/InputCheckCaptcha");
        captchaResponse.EnsureSuccessStatusCode();

        var request = new HttpRequestMessage(HttpMethod.Post, "https://m.cgv.co.kr/Webapp/Member/Login.aspx");
        var form = new Dictionary<string, string>()
        {
            { "hfUserId", Encrypt(id.Trim()) },
            { "hfPasswordInter", ComputeSha256Hash(password) },
            { "hfPasswordLocal", ComputeSha256Hash(ComputeMD5Hash(password)) },
            { "hfReUrl", Encrypt("https %3a%2f%2fm.cgv.co.kr%2f") },
            { "hfAgree", Encrypt("1") },
            { "nonmemberStateCd", Encrypt("0") }
        };
        form = form.Select(x => new KeyValuePair<string, string>(x.Key, Uri.EscapeDataString(x.Value))).ToDictionary();
        request.Content = new FormUrlEncodedContent(form);

        var response = await _authClient.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        if (content.Contains("ID 또는 비밀번호가 일치하지 않습니다"))
            return false;

        var document = new HtmlDocument();
        document.LoadHtml(content);
        var uri = document.DocumentNode.FirstChild.Attributes["src"].Value;
        var setCookieResponse = _authClient.GetAsync(uri);
        return true;
    }

    public async Task Logout()
    {
        await _authClient.GetAsync("https://m.cgv.co.kr/WebApp/Member/Logout.aspx");
        await _authClient.GetAsync("https://m.cgv.co.kr/WebApp/Member/ssoLogout.aspx");
    }

    public async Task<bool> GetLoginStateAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "https://m.cgv.co.kr/WebAPP/MyCgvV4/Manage_PersonalInfo.aspx?check=N");
        var response = await _authClient.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        if (content.Contains("CGV아이디를 입력해주세요."))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    #region Cookies
    public IEnumerable<Cookie> GetAuthCookies()
    {
        return SelectAuthCookies(_authHandler.CookieContainer.GetAllCookies());
    }

    public void SetAuthCookies(IEnumerable<Cookie> cookies)
    {
        ArgumentNullException.ThrowIfNull(cookies, nameof(cookies));
        foreach (var cookie in SelectAuthCookies(cookies))
        {
            _authHandler.CookieContainer.Add(cookie);
        }
    }

    private static IEnumerable<Cookie> SelectAuthCookies(IEnumerable<Cookie> cookies)
    {
        ArgumentNullException.ThrowIfNull(cookies, nameof(cookies));
        var selectedCookies = cookies.Where(x =>
        {
            switch (x.Name)
            {
                case "ASP.NET_SessionId":
                    return true;
                case "WEBAUTH":
                    return true;
                case "AUTOLOGIN":
                    return true;
                case ".ASPXAUTH":
                    return true;
                case "URL_PREV_COMMON":
                    return true;
                case "REURL":
                    return true;
                default: return false;
            }
        });
        if (selectedCookies.Count() < 4)
            return Enumerable.Empty<Cookie>();
        return selectedCookies;
    }
    #endregion
}
