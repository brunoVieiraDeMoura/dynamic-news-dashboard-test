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

public class UserEndpointRequest
{

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet("/users", GetUsers).RequireAuthorization();
        app.MapGet("/users/{id}", GetUser).RequireAuthorization();
        app.MapPost("/users", CreateUser);
        app.MapPost("/login", LoginUser);
        app.MapPut("/users/{id}", EditUser).RequireAuthorization();
        app.MapDelete("/users/{id}", DeleteUser).RequireAuthorization();
    }
    private async Task<IResult> GetUsers([FromServices] AppDbContext db)
    {
        var user = await db.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Password = u.Password,
                Role = u.Role
            })
            .ToListAsync();

        return Results.Ok(user);
    }

    private async Task<IResult> GetUser([FromServices] AppDbContext db, int id)
    {
        var user = await db.Users
            .Where(u => u.Id == id)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Password = u.Password,
                Role = u.Role
            })
            .FirstOrDefaultAsync();

        if (user == null) return Results.NotFound("User not found");

        var userDto = new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Password = user.Password,
            Role = user.Role
        };

        userDto.Posts = await db.Posts
            .Where(p => p.UserId == user.Id)
            .ToListAsync();

        return Results.Ok(userDto);
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



        db.Users.Add(user);

        await db.SaveChangesAsync();

        var userDto = new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Password = user.Password,
            Role = user.Role
        };

        return Results.Ok(userDto);
    }
    private async Task<IResult> LoginUser(
        [FromServices]AppDbContext db,
        [FromServices] IOptions<JwtSettings> jwtSettings,
        [FromBody] LoginDto login)
    {
        var user = db.Users.FirstOrDefault(u =>
            u.Email == login.Email && u.Password == login.Password);

        if (user == null) return Results.Unauthorized();

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(jwtSettings.Value.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            expires: DateTime.UtcNow.AddHours(1),
            claims: claims,
            signingCredentials: creds);


        return Results.Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });


    }
    private async Task<IResult> EditUser([FromServices] AppDbContext db, [FromBody] User userUpdate, int id)
    {
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

        db.Users.Remove(user);

        await db.SaveChangesAsync();

        return Results.Ok($"User {user.Id} deleted");
    }
}
