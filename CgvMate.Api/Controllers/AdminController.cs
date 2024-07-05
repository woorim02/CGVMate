using CgvMate.Api.DTOs;
using CgvMate.Api.Services.Repos;
using CgvMate.Services.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CgvMate.Api.Controllers;

[ApiController]
[Route("admin")]
public class AdminController : Controller
{
    private readonly ILogger<AdminController> _logger;
    private readonly IAdminService _adminService;

    public AdminController(ILogger<AdminController> logger, IAdminService service)
    {
        _logger = logger;
        _adminService = service;
    }

    [HttpPost]
    [Route("login")]
    public IActionResult Login([FromBody] LoginReqDTO request)
    {
        var adminUsername = Environment.GetEnvironmentVariable("ADMIN_ID");
        var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");

        if (request.UserName == adminUsername && request.Password == adminPassword)
        {
            var token = GenerateJwtToken();
            _logger.LogInformation($"[{DateTime.Now}] 로그인 성공 ({request.UserName}/{HttpContext.Connection.RemoteIpAddress})");
            return Ok(new { token });
        }
        _logger.LogWarning($"[{DateTime.Now}] 로그인 실패 ({request.UserName}/{HttpContext.Connection.RemoteIpAddress})");
        return Unauthorized();
    }

    [Authorize]
    [HttpGet]
    [Route("lotte/event/giveaway/keywords")]
    public async Task<IActionResult> GetLotteGiveawayEventKeywords()
    {
        var keywords = await _adminService.GetLotteGiveawayEventKeywords();
        return Ok(keywords);
    }

    [Authorize]
    [HttpPost]
    [Route("lotte/event/giveaway/keywords")]
    public async Task<IActionResult> DeleteLotteGiveawayEventKeyword([FromBody] LotteGiveawayEventKeywordReqDTO dto)
    {
        if (dto.Method == "add")
        {
            await _adminService.AddLotteGiveawayEventKeywords(dto.Keyword);
        }
        else if(dto.Method == "delete")
        {
            await _adminService.DeleteLotteGiveawayEventKeywords(dto.Keyword);
        }
        else
        {
            return BadRequest("unknown Method"); 
        }
        return Ok();
    }

    private string GenerateJwtToken()
    {
        var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(Environment.GetEnvironmentVariable("JWT_KEY")!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
                new Claim(ClaimTypes.Name, Environment.GetEnvironmentVariable("ADMIN_ID")!),
                new Claim(ClaimTypes.Role, "Admin")
        };

        var token = new JwtSecurityToken(
            issuer: "https://cgvmate.com",
            audience: "cgvmate-api",
            claims: claims,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
