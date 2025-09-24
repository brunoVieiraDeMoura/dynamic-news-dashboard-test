
using jornal.Models;
using jornal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace jornal.Controllers
{
    public class CategoryEndpoint
    {
        public void Addroute(IEndpointRouteBuilder app)
        {
            app.MapPost("/category", CreateCategory);
            app.MapGet("/categorys", ReadCategorys);
            app.MapGet("/category/{id}", ReadCategory);
            app.MapPut("/category/{id}", UpdateCategory);
            app.MapDelete("/category/{id}", DeleteCategory);
        }
        private async Task<IResult> CreateCategory([FromServices] AppDbContext db, [FromBody] Category category)
        {
            if (category == null) return Results.BadRequest("Invalid category");
            if (category.Name == null) return Results.BadRequest("Invalid category name");
            if (category.Slug == null) return Results.BadRequest("Invalid category slug");

            db.Categories.Add(category);

            await db.SaveChangesAsync();

            CategoryDto? categoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Slug = category.Slug,
                Date = category.Date
            };

            return Results.Ok(category);
        }
        private async Task<IResult> ReadCategorys([FromServices] AppDbContext db)
        {
            var categorys = await db.Categories.ToListAsync();

            if (categorys.Count == 0) return Results.BadRequest("There are no categories");

            return Results.Ok(categorys);
        }
        private async Task<IResult> ReadCategory([FromServices] AppDbContext db, int id)
        {
            var category = await db.Categories.FindAsync(id);

            if (category == null) return Results.BadRequest("Category not found");

            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Slug = category.Slug,
                Date = category.Date,
                SubCategories = await db.SubCategories
                    .Where(sub => sub.CategoryId == category.Id)
                    .ToListAsync()
            };

            return Results.Ok(categoryDto);
        }
        private async Task<IResult> UpdateCategory([FromServices] AppDbContext db, [FromBody] Category categoryUpdate, int id)
        {
            if (categoryUpdate == null) return Results.BadRequest("Category is null");

            if (categoryUpdate.Name == null && categoryUpdate.Slug == null) return Results.BadRequest("Category Name and Slug is null");

            Category? category = await db.Categories.FindAsync(id);

            if (category == null) return Results.BadRequest("Category not found");

            if (category.Name == null) return Results.BadRequest("Category name is null");

            if (categoryUpdate.Name != null)
                category.Name = categoryUpdate.Name;

            if (category.Slug == null) return Results.BadRequest("Category slug is null");

            if (categoryUpdate.Slug != null)
                category.Slug = categoryUpdate.Slug;

            return Results.Ok(category);
        }
        private async Task<IResult> DeleteCategory([FromServices] AppDbContext db, int id)
        {
            var category = await db.Categories.FindAsync(id);

            if (category == null) return Results.BadRequest("Invalid Category Id");

            await db.SaveChangesAsync();

            return Results.Ok($"Category {id} Deleted");
        }
    }
}
