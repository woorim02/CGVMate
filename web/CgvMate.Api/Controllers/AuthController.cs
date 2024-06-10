using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CgvMate.Api;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    static SHA256 sha256 = SHA256.Create();
    AppDbContext context;
    public AuthController(AppDbContext context)
    {
        this.context = context;
    }

    [HttpPost("signin")]
    public async Task<IActionResult> LoginAsync([FromForm] string username, [FromForm] string password)
    {
        var foundUser = await context.Users.FirstOrDefaultAsync(x => x.UserName == username);
        if (foundUser is null)
            return Unauthorized("아이디 또는 비빌번호가 잘못되었습니다.");
        var passwordHash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
        if (foundUser.PasswordHash != passwordHash)
            return Unauthorized("아이디 또는 비밀번호가 잘못되었습니다.");

        return Ok();
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUpAsync([FromForm] string username, [FromForm] string password)
    {
        var foundUser = await context.Users.FirstOrDefaultAsync(x => x.UserName == username);
        if (foundUser is not null)
            return Unauthorized("이미 사용중인 아이디입니다.");
        if (!ValidatePassword(password))
            return Unauthorized("비밀번호는 8자리 이상, 숫자, 대문자, 특수문자 각각 1자리를 포함하여야 합니다.");

        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        var passwordHash = Convert.ToBase64String(hash);

        User user = new User
        {
            IsAdmin = false,
            UserName = username,
            PasswordHash = passwordHash,
        };

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return Ok();
    }

    static bool ValidatePassword(string password)
    {
        if (password.Length < 8)
            return false;

        bool hasUpperCase = password.Any(char.IsUpper);
        if (!hasUpperCase)
            return false;

        bool hasDigit = password.Any(char. IsDigit);
        if (!hasDigit)
            return false;

        bool hasSpecialChar = password.Any(ch => !char.IsLetterOrDigit(ch));
        if (!hasSpecialChar)
            return false;

        return true;
    }
}
