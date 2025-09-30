
using jornal.Models;
using jornal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace jornal.Controllers;

public class LoginEndpoint
{
    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost("/login", Login);
    }
    private async Task<IResult> Login(
        [FromServices] AppDbContext db,
        [FromServices] IOptions<JwtSettings> jwtSettings,
        [FromBody] LoginDto login)
    {

        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == login.Email);
        if (user == null) return Results.Unauthorized();

        bool passwordOk = BCrypt.Net.BCrypt.Verify(login.Password, user.Password);
        if (!passwordOk) return Results.Unauthorized();

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings.Value.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            expires: DateTime.UtcNow.AddHours(24),
            claims: claims,
            signingCredentials: creds);

        return Results.Ok(token);
    }
}
