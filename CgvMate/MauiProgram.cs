using System.Net;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace CgvMate
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
            var handler = new SocketsHttpHandler() { CookieContainer = new CookieContainer() };
            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0;) Chrome/120.0.0.0 Safari/537.36");
            client.DefaultRequestHeaders.Host = "m.cgv.co.kr";

            var authHandler = new SocketsHttpHandler() { CookieContainer = new CookieContainer() };
            var authClient = new HttpClient(authHandler);
            authClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0;) Chrome/120.0.0.0 Safari/537.36");
            authClient.DefaultRequestHeaders.Host = "m.cgv.co.kr";
            var service = new CgvService(client, authClient, handler, authHandler);

            builder.Services.AddSingleton<CgvService>(service);

            using var db = new AppDbContext();
            db.Database.EnsureCreated();

            return builder.Build();
        }
    }
}
