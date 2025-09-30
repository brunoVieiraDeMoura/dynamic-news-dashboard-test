using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using jornal.Models;
using jornal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MiniValidation;

namespace jornal.Controllers;

public class UserEndpoint
{
    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost("/user", Create);
        app.MapPost("/user/google", CreateGoogle);
        app.MapGet("/users", Reads).RequireAuthorization();
        app.MapGet("/user/{id}", Read).RequireAuthorization();
        app.MapPut("/user/{id}", Update).RequireAuthorization(new AuthorizeAttribute { Roles = "admin" });
        app.MapDelete("/user/{id}", Delete).RequireAuthorization();
        app.MapPost("/user/me", Me).RequireAuthorization();
    }
    private async Task<IResult> Create(
        [FromServices] AppDbContext db,
        [FromBody] UserCreateDto dto)
    {
        if (dto == null) return Results.BadRequest("Invalid user");
        if (dto.Name == null) return Results.BadRequest("Invalid user name");
        if (dto.Email == null) return Results.BadRequest("Invalid user email");
        if (dto.Password == null) return Results.BadRequest("invalid user Password");

        if (!MiniValidator.TryValidate(dto, out var errors))
            return Results.ValidationProblem(errors);

        if (await db.Users.AnyAsync(u => u.Email == dto.Email)) return Results.BadRequest("Email already registered");

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        await db.Users.AddAsync(user);
        await db.SaveChangesAsync();

        return Results.Ok($"Usuario criado com sucesso!");

    }
    private async Task<IResult> CreateGoogle(
        [FromServices] AppDbContext db,
        [FromBody] UserGoogle dto,
        IConfiguration config)
    {
        if (string.IsNullOrEmpty(dto.IdToken))
            return Results.BadRequest("Invalid Google Token");

        var payload = await Google.Apis.Auth.GoogleJsonWebSignature.ValidateAsync(dto.IdToken);

        if (payload == null || string.IsNullOrEmpty(payload.Email))
            return Results.BadRequest("Invalid Google Token");

        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == payload.Email);

        if (user == null)
        {
            user = new User
            {
                Name = payload.Name ?? payload.Email,
                Email = payload.Email,
                Password = null,
                Role = "user"
            };

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, user.Role ?? "user")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: creds);

        var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return Results.Ok(token);
    }
    private async Task<IResult> Reads([FromServices] AppDbContext db)
    {
        var user = await db.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                Date = u.Date,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role
            })
            .ToListAsync();

        if (user.Count == 0) return Results.BadRequest("User count is 0");

        return Results.Ok(user);
    }
    private async Task<IResult> Read([FromServices] AppDbContext db, int id)
    {
        var user = await db.Users
            .Where(u => u.Id == id)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Date = u.Date,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role
            })
            .FirstOrDefaultAsync();

        if (user == null) return Results.NotFound("User not found");

        user.Posts = await db.Posts
            .Where(p => p.UserId == user.Id)
            .ToListAsync();

        return Results.Ok(user);
    }

    private async Task<IResult> Update([FromServices] AppDbContext db, [FromBody] User userUpdate, int id)
    {
        if (userUpdate == null) return Results.BadRequest("User is null");

        if (!await db.Users.AnyAsync(u => u.Email == userUpdate.Email)) return Results.BadRequest("Email already registered");

        var user = await db.Users.FindAsync(id);

        if (user == null) return Results.BadRequest("Invalid user");

        if (userUpdate.Name == null &&
            userUpdate.Email == null &&
            userUpdate.Password == null &&
            userUpdate.Role == null)
            return Results.BadRequest("All values is null");

        if (userUpdate.Name != null) user.Name = userUpdate.Name;
        if (userUpdate.Email != null) user.Email = userUpdate.Email;
        if (userUpdate.Password != null) user.Password = userUpdate.Password;

        if (userUpdate.Role != null)
        {
            if (userUpdate.Role == "user" ||
                userUpdate.Role == "admin" ||
                userUpdate.Role == "writer" ||
                userUpdate.Role == "review"
                )
            {
                user.Role = userUpdate.Role;
            }
            else
            {
                return Results.BadRequest("Invalid role");
            }
        }

        await db.SaveChangesAsync();

        return Results.Ok(user);
    }

    private async Task<IResult> Delete([FromServices] AppDbContext db, int id)
    {
        var user = await db.Users.FindAsync(id);

        if (user == null)
            return Results.BadRequest("User not found");

        db.Users.Remove(user);

        await db.SaveChangesAsync();

        return Results.Ok($"User {user.Id} deleted");
    }
    private async Task<IResult> Me(
        [FromServices] AppDbContext db,
        HttpContext http)
    {
        var emailClaim = http.User.FindFirst(ClaimTypes.Name)?.Value;

        if (emailClaim == null)
            return Results.Unauthorized();

        var user = await db.Users
            .Where(u => u.Email == emailClaim)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Date = u.Date,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role
            })
            .FirstOrDefaultAsync();

        if (user == null)
            return Results.NotFound("User not found");

        return Results.Ok(user);
    }
}
