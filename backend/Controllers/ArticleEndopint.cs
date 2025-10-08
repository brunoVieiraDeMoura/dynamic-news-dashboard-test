
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
    private async Task<IResult> ArticleCreate(
        [FromServices] AppDbContext db,
        [FromBody] ArticleCreateDto articleCreate)
    {
        if (articleCreate == null) return Results.BadRequest("Article is null");

        var category = await db.Categories.FindAsync(articleCreate.CategoryId);

        if (category == null) return Results.BadRequest("Category not found");

        var subCategory = await db.SubCategories.FindAsync(articleCreate.SubCategoryId);

        if (subCategory == null) return Results.BadRequest("SubCategory not found");

        var article = new Article
        {
            CategoryId = articleCreate.CategoryId,
            SubCategoryId = articleCreate.SubCategoryId,
            Category = category.Name,
            SubCategory = subCategory.Name,
            Accepted = articleCreate.Accepted,
            Post = articleCreate.Post,
            ReviewID = articleCreate.ReviewID,
            Slug = articleCreate.Slug,
            Title = articleCreate.Title,
            WriterID = articleCreate.WriterID
        };

        await db.Articles.AddAsync(article);

        await db.SaveChangesAsync();

        return Results.Ok(article);
    }
    private async Task<IResult> ArticlesRead([FromServices] AppDbContext db)
    {
        if (!db.Articles.Any()) return Results.BadRequest("User count is 0");

        var articles = await db.Articles.ToListAsync();

        foreach (var article in articles)
        {
            article.Comments = await db.Messages
                .Where(m => m.ArticleId == article.Id)
                .ToListAsync();
        }

        return Results.Ok(articles);
    }
    private async Task<IResult> ArticleRead(
        [FromServices] AppDbContext db,
        int id)
    {
        var article = await db.Articles
            .Where(a => a.Id == id)
            .FirstOrDefaultAsync();

        if (article == null) return Results.BadRequest("Article not found");

        article.Comments = await db.Messages
            .Where(m => m.ArticleId == article.Id)
            .ToListAsync();

        return Results.Ok(article);
    }

    private async Task<IResult> ArticleUpdate(
        AppDbContext db,
        ArticleUpdateDto articleUpdateDto,
        int id)
    {
        var article = await db.Articles.FindAsync(id);
        if (article == null) return Results.BadRequest("Article not found");

        if (articleUpdateDto.Title == null ||
            articleUpdateDto.Slug == null ||
            articleUpdateDto.Post == null)
            return Results.BadRequest("All values is null");

        if (articleUpdateDto.Title != null)
            article.Title = articleUpdateDto.Title;
        if (articleUpdateDto.Slug != null)
            article.Slug = articleUpdateDto.Slug;
        if (articleUpdateDto.Post != null)
            article.Post = articleUpdateDto.Post;

        await db.SaveChangesAsync();

        return Results.Ok(articleUpdateDto);
    }

    private async Task<IResult> ArticleDelete(
        AppDbContext db,
        int id)
    {
        var article = await db.Articles.FindAsync(id);

        if (article == null) return Results.BadRequest("Article not found");

        db.Articles.Remove(article);

        await db.SaveChangesAsync();

        return Results.Ok("Sucessfull");
    }
}
