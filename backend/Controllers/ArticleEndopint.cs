
using jornal.Models.Article;
using jornal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace jornal.Controllers;
public class ArticleEndopint
{
    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost("/article", ArticleCreate);
        app.MapGet("/articles", ArticlesRead);
        app.MapGet("/article/{id}", ArticleRead);
        app.MapPut("/article/{id}", ArticleUpdate);
        app.MapDelete("/article/{id}", ArticleDelete);

    }
    private async Task<IResult> ArticleCreate([FromServices] AppDbContext db, [FromBody] Article article)
    {
        if (article == null) return Results.BadRequest("Article is null");

        await db.Articles.AddAsync(article);

        await db.SaveChangesAsync();

        return Results.Ok(article);
    }
    private async Task<IResult> ArticlesRead([FromServices] AppDbContext db)
    {
        if (!db.Articles.Any()) return Results.BadRequest("User count is 0");

        var articles = await db.Articles.ToListAsync();

        return Results.Ok(articles);
    }
    private async Task<IResult> ArticleRead([FromServices] AppDbContext db, int id)
    {
        var article = await db.Articles
            .Where(a => a.Id == id)
            .FirstOrDefaultAsync();

        if (article == null) return Results.BadRequest("Article not found");

        return Results.Ok(article);
    }

    private async Task ArticleUpdate(HttpContext context)
    {
        throw new NotImplementedException();
    }

    private async Task<IResult> ArticleDelete(AppDbContext db, int id)
    {
        var article = await db.Articles.FindAsync(id);

        if (article == null) return Results.BadRequest("Article not found");

        db.Articles.Remove(article);

        await db.SaveChangesAsync();

        return Results.Ok("Sucessfull");
    }
}
