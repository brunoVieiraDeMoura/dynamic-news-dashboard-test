using jornal.Models;
using jornal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace jornal.Controllers;

public class UserEndpointRequest
{
    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapGet("/users", GetUsers);
        app.MapGet("user/{id}", GetUser);
        app.MapPost("user", CreateUser);
        app.MapPut("user", EditUser);
        app.MapDelete("user/{id}", DeleteUser);

    }

    private async Task<IResult> GetUsers([FromServices] AppDbContext db)
    {
        var user = await db.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Password = u.Password
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
                Email = u.Email
            })
            .FirstOrDefaultAsync();

        if (user == null)
            return Results.NotFound("User not found");

        return Results.Ok(user);
    }

    private async Task<IResult> CreateUser([FromServices] AppDbContext db, [FromBody] User user)
    {
        if (user == null)
            return Results.BadRequest("Invalid user");

        db.Users.Add(user);

        try
        {
            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex);
        }

        var userDto = new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Password = user.Password
        };
        



        return Results.Ok(userDto);

    }

    private async Task<IResult> EditUser([FromServices] AppDbContext db, [FromBody] User userUpdate)
    {
        var user = await db.Users.FindAsync(userUpdate.Id);

        if (user == null)
            return Results.BadRequest("Invalid user");

        user.Name = userUpdate.Name;
        user.Email = userUpdate.Email;
        user.Password = userUpdate.Password;
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
