using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using jornal.Models;
using jornal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace jornal.Controllers;

public class UserEndpoint
{

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost("/user", CreateUser);
        app.MapGet("/users", ReadUsers).RequireAuthorization();
        app.MapGet("/user/{id}", ReadUser).RequireAuthorization();
        app.MapPut("/user/{id}", UpdateUser).RequireAuthorization();
        app.MapDelete("/user/{id}", DeleteUser).RequireAuthorization();
        app.MapPost("/login", Login);
        app.MapPost("/me", Me).RequireAuthorization();
    }
    private async Task<IResult> CreateUser(
        [FromServices] AppDbContext db,
        [FromBody] User user)
    {
        if (user == null)
            return Results.BadRequest("Invalid user");
        if (user.Name == null)
            return Results.BadRequest("Invalid user name");
        if (user.Email == null)
            return Results.BadRequest("Invalid user email");
        if (user.Password == null)
            return Results.BadRequest("invalid user Password");

        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        db.Users.Add(user);

        await db.SaveChangesAsync();

        var userDto = new UserDto
        {
            Id = user.Id,
            Date = user.Date,
            Name = user.Name,
            Email = user.Email,
            Password = user.Password,
            Role = user.Role
        };

        

        return Results.Ok($"Usuario criado com sucesso!" );
    }
    private async Task<IResult> ReadUsers([FromServices] AppDbContext db)
    {
        var user = await db.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                Date = u.Date,
                Name = u.Name,
                Email = u.Email,
                Password = u.Password,
                Role = u.Role
            })
            .ToListAsync();

        if (user.Count == 0) return Results.BadRequest("User count is 0");

        return Results.Ok(user);
    }
    private async Task<IResult> ReadUser([FromServices] AppDbContext db, int id)
    {
        var user = await db.Users
            .Where(u => u.Id == id)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Date = u.Date,
                Name = u.Name,
                Email = u.Email,
                Password = u.Password,
                Role = u.Role
            })
            .FirstOrDefaultAsync();

        if (user == null) return Results.NotFound("User not found");

        user.Posts = await db.Posts
            .Where(p => p.UserId == user.Id)
            .ToListAsync();

        return Results.Ok(user);
    }

    private async Task<IResult> UpdateUser([FromServices] AppDbContext db, [FromBody] User userUpdate, int id)
    {
        if (userUpdate == null) return Results.BadRequest("User is null");

        var user = await db.Users.FindAsync(id);

        if (user == null)
            return Results.BadRequest("Invalid user");

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

    private async Task<IResult> DeleteUser([FromServices] AppDbContext db, int id)
    {
        var user = await db.Users.FindAsync(id);

        if (user == null)
            return Results.BadRequest("User not found");

        db.Users.Remove(user);

        await db.SaveChangesAsync();

        return Results.Ok($"User {user.Id} deleted");
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

        var res = new
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            User = new
            {
                user.Name,
                user.Role
            }
        };

        return Results.Ok(res);
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
