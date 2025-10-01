using jornal.Models;
using jornal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace jornal.Controllers;
public class PostEndpoint
{
    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost("/post", CreatePost);
        app.MapGet("/posts", ReadPosts);
        app.MapGet("/post/{id}", ReadPost);
        app.MapPut("/post/{id}", UpdatePost);
        app.MapDelete("/post/{id}", DeletePost);
    }
    private async Task<IResult> CreatePost([FromServices] AppDbContext db, [FromBody] Post post)
    {
        if (post == null) return Results.BadRequest("Invalid post");

        if (post.Text == null) return Results.BadRequest("Invalid post text");
        if (post.Title == null) return Results.BadRequest("Invalid post title");

        var user = await db.Users.FindAsync(post.UserId);
        if (user == null)
            return Results.BadRequest("UserId not found");

        db.Posts.Add(post);

        await db.SaveChangesAsync();

        var postDto = new PostDto
        {
            Id = post.Id,
            Date = post.Date,
            UserId = post.UserId,
            Title = post.Title,
            Text = post.Text,
            UserName = user.Name
        };

        return Results.Ok(postDto);
    }
    private async Task<IResult> ReadPosts([FromServices] AppDbContext db)
    {
        var posts = await db.Posts
            .Include(p => p.User)
            .Select(p => new PostDto
            {
                Id = p.Id,
                Date = p.Date,
                UserId = p.UserId,
                Title = p.Title,
                Text = p.Text,
                UserName = p.User.Name
            })
            .ToListAsync();

        if (posts.Count == 0) return Results.BadRequest("Postos count is 0");

        return Results.Ok(posts);
    }
    private async Task<IResult> ReadPost([FromServices] AppDbContext db, int id)
    {
        var post = await db.Posts
            .Include(p => p.User)
            .Where(p => p.Id == id)
            .Select(p => new PostDto
            {
                Id = p.Id,
                Date = p.Date,
                UserId = p.UserId,
                Title = p.Title,
                Text = p.Text,
                UserName = p.User.Name
            })
            .FirstOrDefaultAsync();

        if (post == null)
            return Results.NotFound("Post not found");

        return Results.Ok(post);
    }
    private async Task<IResult> UpdatePost([FromServices] AppDbContext db, [FromBody] Post postUpdate, int id)
    {
        if (postUpdate == null) return Results.BadRequest("Post is null");

        if (postUpdate.Title == null && postUpdate.Text == null) return Results.BadRequest("Post Title and Text is null");
        
        var post = await db.Posts.FindAsync(id);
        if (post == null)
            return Results.BadRequest("Post not found");

        if (postUpdate.Title != null) post.Title = postUpdate.Title;
        if (postUpdate.Text != null) post.Text = postUpdate.Text;

        await db.SaveChangesAsync();

        var user = await db.Users.FindAsync(post.UserId);

        if (user == null) user.Name = "null";

        var postDto = new PostDto
        {
            Id = post.Id,
            Date = post.Date,
            UserId = post.UserId,
            Title = post.Title,
            Text = post.Text,
            UserName = user.Name
        };

        return Results.Ok(postDto);
    }
    private async Task<IResult> DeletePost([FromServices] AppDbContext db, int id)
    {
        var post = await db.Posts.FindAsync(id);
        if (post == null)
            return Results.NotFound("Post not found");

        db.Posts.Remove(post);
        await db.SaveChangesAsync();

        return Results.Ok($"Post {post.Id} deleted");
    }
}
