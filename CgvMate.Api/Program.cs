
using CgvMate.Api.Data;
using CgvMate.Api.Middleware;
using CgvMate.Api.Services;
using CgvMate.Api.Services.Repos;
using CgvMate.Services;
using CgvMate.Services.Repos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Net;
using System.Text;

namespace CgvMate.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc($"v1", new OpenApiInfo { Title = "Cgvmate Api", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                    new string[]{}
                    }   
                });
            });

            // get Enviroment Varables
#if DEBUG
            SetTestEnvironmentVariable();
#endif
            string? ADMIN_ID = Environment.GetEnvironmentVariable("ADMIN_ID");
            string? ADMIN_PASSWORD = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");
            string? JWT_KEY = Environment.GetEnvironmentVariable("JWT_KEY");
            string? IV = Environment.GetEnvironmentVariable("IV");
            string? KEY = Environment.GetEnvironmentVariable("KEY");
            string? CONNECTION_STRING = Environment.GetEnvironmentVariable("CONNECTION_STRING");

            if (IV == null
                || KEY == null
                || CONNECTION_STRING == null
                || JWT_KEY == null
                || ADMIN_ID == null
                || ADMIN_PASSWORD == null)
            {
                throw new Exception("환경변수를 모두 설정해 주세요");
            }

            #region App
            // Add Auth
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "https://cgvmate.com",
                        ValidAudience = "cgvmate-api",
                        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(JWT_KEY))
                    };
                });

            builder.Services.AddAuthorization();

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

            builder.Services.AddScoped<IAdminService, AdminService>(provider =>
            {
                var lotteGiveawayEventKeywordRepository = provider.GetRequiredService<ILotteGiveawayEventKeywordRepository>();
                return new AdminService(lotteGiveawayEventKeywordRepository);
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

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
        
        private static void SetTestEnvironmentVariable()
        {
            Environment.SetEnvironmentVariable("ADMIN_ID", "admin");
            Environment.SetEnvironmentVariable("ADMIN_PASSWORD", "password");
            Environment.SetEnvironmentVariable("JWT_KEY", "E7qVMZftP0q9zIsyja8+24/CLGY6KcPIhyevEDu8qL4=");
            Environment.SetEnvironmentVariable("IV", "x3nV9mvvsS+qIN1h2uVEviEFO2M+KXDWw66ClG1fFu4=");
            Environment.SetEnvironmentVariable("KEY", "n8eqQYpQH816E2tfcvqA+A==");
            Environment.SetEnvironmentVariable("CONNECTION_STRING", "Server=192.168.0.51;Port=13306;Database=cgvmate;User=root;Password=password;");
        }
    }
}
