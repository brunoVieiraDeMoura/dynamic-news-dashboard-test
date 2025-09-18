using jornal.Models;
using jornal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace jornal.Controllers
{
    public class PostEndpointRequest
    {
        public void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapGet("/posts", GetPosts);
            app.MapGet("/posts/{id}", GetPost);
            app.MapPost("/posts", CreatePost);
            app.MapPut("/posts/{id}", EditPost);
            app.MapDelete("/posts/{id}", DeletePost);
        }

        private async Task<IResult> GetPosts([FromServices] AppDbContext db)
        {
            var posts = await db.Posts
                .Include(p => p.User)
                .Select(p => new PostDto
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    Title = p.Title,
                    Text = p.Text,
                    UserName = p.User.Name
                })
                .ToListAsync();

            return Results.Ok(posts);
        }

        private async Task<IResult> GetPost([FromServices] AppDbContext db, int id)
        {
            var post = await db.Posts
                .Include(p => p.User)
                .Where(p => p.Id == id)
                .Select(p => new PostDto
                {
                    Id = p.Id,
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

        private async Task<IResult> CreatePost([FromServices] AppDbContext db, [FromBody] Post post)
        {
            if (post == null)
                return Results.BadRequest("Invalid post");

            if (post.Text == null)
                return Results.BadRequest("Invalid post text");
            if (post.Title == null)
                return Results.BadRequest("Invalid post title");

            var user = await db.Users.FindAsync(post.UserId);
            if (user == null)
                return Results.BadRequest("UserId not found");

            db.Posts.Add(post);

            await db.SaveChangesAsync();

            var postDto = new PostDto
            {
                Id = post.Id,
                UserId = post.UserId,
                Title = post.Title,
                Text = post.Text,
                UserName = user.Name
            };

            return Results.Ok(postDto);

        }

        private async Task<IResult> EditPost([FromServices] AppDbContext db, [FromBody] Post postUpdate, int id)
        {
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
                UserId = post.UserId,
                Title = post.Title,
                Text = post.Text,
                UserName = user.Name
            };

            return Results.Ok(postDto);
        }

        private async Task<IResult> DeletePost([FromServices] AppDbContext db,int id)
        {
            var post = await db.Posts.FindAsync(id);
            if (post == null)
                return Results.NotFound("Post not found");

            db.Posts.Remove(post);
            await db.SaveChangesAsync();

            return Results.Ok($"Post {post.Id} deleted");

        }
    }
}
