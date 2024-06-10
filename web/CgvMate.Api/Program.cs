using System.Net;
using LotteMate;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace CgvMate.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

#region Singleton
        var handler = new SocketsHttpHandler() { CookieContainer = new CookieContainer() };
        var client = new HttpClient(handler);
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0;) Chrome/120.0.0.0 Safari/537.36");
        client.DefaultRequestHeaders.Host = "m.cgv.co.kr";
        var authHandler = new SocketsHttpHandler() { CookieContainer = new CookieContainer() };
        var authClient = new HttpClient(authHandler);
        authClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0;) Chrome/120.0.0.0 Safari/537.36");
        authClient.DefaultRequestHeaders.Host = "m.cgv.co.kr";
        var service = new CgvService(client, authClient, handler, authHandler);

        var lotteClient = new HttpClient();
        var lotteService = new LotteService(lotteClient);

        builder.Services.AddSingleton(service);
        builder.Services.AddSingleton(client);
        builder.Services.AddSingleton(lotteService);
#endregion

#region DataBase
        string? connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
        #if DEBUG
        connectionString = "Server=localhost;Port=13307;Database=cgvmate;User=cgvmate;Password=password;";
        #endif
        if (connectionString == null)
            throw new Exception("connection string is null");
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });
#endregion

#region Auth
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromHours(48);
            options.LoginPath = "/auth/login";
            options.LogoutPath = "/auth/logout";
            options.AccessDeniedPath = "/";
        });
#endregion

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseAuthorization();
        app.UseAuthentication();
        app.MapControllers();

        app.Run();
    }
}

