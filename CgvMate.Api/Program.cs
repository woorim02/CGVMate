
using CgvMate.Api.Data;
using CgvMate.Api.Middleware;
using CgvMate.Services;
using CgvMate.Services.Repos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;

namespace CgvMate.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // get Enviroment Varables
#if DEBUG
            string? IV = "";
            string? KEY = "";
            string? CONNECTION_STRING = "Server=192.168.0.51;Port=13306;Database=cgvmate;User=root;Password=password; ";
#else
            string? IV = Environment.GetEnvironmentVariable("IV");
            string? KEY = Environment.GetEnvironmentVariable("KEY");
            string? CONNECTION_STRING = Environment.GetEnvironmentVariable("CONNECTION_STRING");

            if (IV == null && KEY == null && CONNECTION_STRING == null)
            {
                throw new Exception("환경변수를 모두 설정해 주세요");
            }
#endif


            #region App
            // Add DbContext
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(CONNECTION_STRING, ServerVersion.AutoDetect(CONNECTION_STRING));
            });

            // Add repositories
            builder.Services.AddScoped<ICgvGiveawayEventRepository, CgvGiveawayEventRepository>();
            builder.Services.AddScoped<ILotteGiveawayEventRepository, LotteGiveawayEventRepository>();
            builder.Services.AddScoped<ILotteGiveawayEventKeywordRepository, LotteGiveawayEventKeywordRepository>();
            builder.Services.AddScoped<IMegaboxGiveawayEventRepository, MegaboxGiveawayEventRepository>();
            builder.Services.AddScoped<ILotteGiveawayEventModelRepository, LotteGiveawayEventModelRepository>();

            // Add HttpClient
            builder.Services.AddHttpClient();

            //Add Services
            var cgvClient = new HttpClient();
            cgvClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0;) Chrome/120.0.0.0 Safari/537.36");
            cgvClient.DefaultRequestHeaders.Host = "m.cgv.co.kr";
            builder.Services.AddScoped(provider =>
            {
                var giveawayEventRepository = provider.GetRequiredService<ICgvGiveawayEventRepository>();
                return new CgvEventService(cgvClient, giveawayEventRepository, IV, KEY);
            });
            builder.Services.AddScoped(provider =>
            {
                var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient();
                var giveawayEventRepository = provider.GetRequiredService<ILotteGiveawayEventRepository>();
                var lotteGiveawayEventKeywordRepository = provider.GetRequiredService<ILotteGiveawayEventKeywordRepository>();
                var lotteGiveawayEventModelRepository = provider.GetRequiredService<ILotteGiveawayEventModelRepository>();
                return new LotteService(httpClient, giveawayEventRepository, lotteGiveawayEventKeywordRepository, lotteGiveawayEventModelRepository);
            });
            builder.Services.AddScoped(provider =>
            {
                var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient();
                var giveawayEventRepository = provider.GetRequiredService<IMegaboxGiveawayEventRepository>();
                return new MegaboxEventService(httpClient, giveawayEventRepository);
            });
            #endregion

            #region Log
            // 기존 로깅 제공자를 제거하고 콘솔 로깅을 추가
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.SetMinimumLevel(LogLevel.Information);

            // 로깅 끄기
            builder.Logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.None);
            builder.Logging.AddFilter("System.Net.Http.HttpClient", LogLevel.Warning);
            #endregion

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
