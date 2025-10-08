using jornal.Models.Article;
using jornal.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace jornal.Controllers;
public class MessageEndpoint
{
    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost("/message", MessageCreate);
        app.MapGet("/messages", MessagesRead);
        app.MapGet("/message/{id}", MessageRead);
        app.MapPut("/message/{id}", MessageUpdate);
        app.MapDelete("/message/{id}", MessageDelete);
    }

    private async Task<IResult> MessageCreate(AppDbContext db, MessageCreat msg)
    {
        if (msg == null) return Results.BadRequest("Message Is null");

        if (msg.Text == null) return Results.BadRequest("Message Text is null");

        var user = await db.Users.FindAsync(msg.UserId);

        if (user == null) return Results.BadRequest("Messsage UserId not found");

        var article = await db.Articles.FindAsync(msg.ArticleId);

        if (article == null) return Results.BadRequest("Message Article not found");

        Message message = new()
        {
            ArticleId = msg.ArticleId,
            Text = msg.Text,
            UserId = user.Id
        };

        await db.Messages.AddAsync(message);

        await db.SaveChangesAsync();

        return Results.Ok(message);
    }

    private async Task<IResult> MessagesRead(AppDbContext db)
    {
        if (db.Messages.Count() == 0) return Results.BadRequest("Message Count Is 0");

        var messages = await db.Messages.ToListAsync();

        return Results.Ok(messages);
    }

    private async Task<IResult> MessageRead(AppDbContext db, int id)
    {
        var message = await db.Messages.FindAsync(id);

        if (message == null) return Results.BadRequest("Message not found");

        return Results.Ok(message);
    }

    private async Task<IResult> MessageUpdate(AppDbContext db, int id,[FromBody] MessageUpt textUpdate)
    {
        if (textUpdate == null) return Results.BadRequest("Text is null");

        var msg = await db.Messages.FindAsync(id);

        if (msg == null) return Results.BadRequest("Message is not found");

        msg.Text = textUpdate.text;

        await db.SaveChangesAsync();

        return Results.Ok(msg);
    }

    private async Task<IResult> MessageDelete(AppDbContext db, int id)
    {
        var msg = await db.Messages.FindAsync(id);

        if (msg == null) return Results.BadRequest("Message id null");

        db.Messages.Remove(msg);

        await db.SaveChangesAsync();

        return Results.Ok($"Message {id} deleted");
    }
}
