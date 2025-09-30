
using jornal.Models;
using jornal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace jornal.Controllers;
public class SubCategoryEndpoint
{
    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost("/subcategory", CreateSubCategory);
        app.MapGet("/subcategorys", ReadSubCategorys);
        app.MapGet("/subcategory/{id}", ReadSubCategory);
        app.MapPut("/subcategory/{id}", UpdateSubCategory);
        app.MapDelete("/subcategory/{id}", DeleteSubCategory);
    }
    private async Task<IResult> CreateSubCategory([FromServices] AppDbContext db, [FromBody] SubCategory subCategory)
    {
        if (subCategory == null) return Results.BadRequest("SubCategory is null");

        if (subCategory.Name == null) return Results.BadRequest("SubCategory name is null");
        if (subCategory.Slug == null) return Results.BadRequest("SubCategory slug is null");

        Category category = await db.Categories
            .Where(c => c.Id == subCategory.CategoryId)
            .FirstAsync();

        if (category == null) return Results.BadRequest("CategoryId not found");

        subCategory.Category = category;

        await db.SubCategories.AddAsync(subCategory);
        await db.SaveChangesAsync();

        SubCategoryDto subCategoryDto = new SubCategoryDto
        {
            Id = subCategory.Id,
            CategoryId = subCategory.CategoryId,
            Category = subCategory.Category,
            Name = subCategory.Name,
            Slug = subCategory.Slug,
            Date = subCategory.Date
        };

        return Results.Ok(subCategoryDto);
    }
    private async Task<IResult> ReadSubCategorys([FromServices] AppDbContext db)
    {
        var subCategories = await db.SubCategories
            .Include(sub => sub.Category)
            .Select(sub => new SubCategoryDto
            {
                Id = sub.Id,
                CategoryId = sub.CategoryId,
                Name = sub.Name,
                Slug = sub.Slug,
                Date = sub.Date
            })
            .ToListAsync();

        if (subCategories.Count == 0) return Results.BadRequest("SubCategories count ís 0");

        return Results.Ok(subCategories);
    }
    private async Task<IResult> ReadSubCategory([FromServices] AppDbContext db, int id)
    {
        var subCategory = await db.SubCategories
            .Include(sub => sub.Category)
            .Where(sub => sub.CategoryId == id)
            .Select(sub => new SubCategoryDto
            {
                Id = sub.Id,
                CategoryId = sub.CategoryId,
                Name = sub.Name,
                Slug = sub.Slug,
                Date = sub.Date,
            })
            .FirstOrDefaultAsync();

        if (subCategory == null) return Results.BadRequest("Invalid Id subCategory");

        return Results.Ok(subCategory);
    }
    private async Task<IResult> UpdateSubCategory(AppDbContext db,int id, SubCategory subCategoryUpdate)
    {
        if (subCategoryUpdate == null) return Results.BadRequest("SubCategory is null");

        if (subCategoryUpdate.Name == null && subCategoryUpdate.Slug == null) return Results.BadRequest("SubCategory Name and Slug is null");

        var subCategory = await db.Categories.FindAsync(id);

        if (subCategory == null) return Results.BadRequest("subCategory not found");

        if (subCategoryUpdate.Name != null)
            subCategory.Name = subCategoryUpdate.Name;
        if(subCategoryUpdate.Slug != null)
            subCategory.Slug = subCategoryUpdate.Slug;

        await db.SaveChangesAsync();

        return Results.Ok(subCategory);
    }
    private async Task<IResult> DeleteSubCategory(AppDbContext db, int id)
    {
        var subCategory = await db.SubCategories.FindAsync(id);

        if (subCategory == null) return Results.BadRequest("SubCategory not found");

        db.SubCategories.Remove(subCategory);

        await db.SaveChangesAsync();

        return Results.Ok($"SubCategory Id:{subCategory.Id} Deleted");

    }
}
