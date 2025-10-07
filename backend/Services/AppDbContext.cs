using System.Text.Json;
using jornal.Models;
using jornal.Models.Article;
using jornal.Models.Category;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace jornal.Services;

internal class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<SubCategory> SubCategories => Set<SubCategory>();
    public DbSet<Article> Articles { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Monta User com Posts e Deletando em cascata
        modelBuilder.Entity<User>()
            .HasMany(u => u.Posts)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Monta Category com SubCategorys e Deletando em cascata
        modelBuilder.Entity<Category>()
            .HasMany(c => c.SubCategories)
            .WithOne(sub => sub.Category)
            .HasForeignKey(sub => sub.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Converter JSON para o campo Document (Post)
        var documentConverter = new ValueConverter<Document, string>(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => JsonSerializer.Deserialize<Document>(v, (JsonSerializerOptions?)null) ?? new Document()
        );

        // Converter JSON para listas (Comments, etc.)
        var messageListConverter = new ValueConverter<List<Message>?, string>(
            v => JsonSerializer.Serialize(v ?? new List<Message>(), (JsonSerializerOptions?)null),
            v => JsonSerializer.Deserialize<List<Message>>(v, (JsonSerializerOptions?)null) ?? new List<Message>()
        );

        // Article
        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Title).IsRequired();
            entity.Property(a => a.Slug).IsRequired();

            // Salvar Document (Post) como JSON
            entity.Property(a => a.Post)
                .HasConversion(documentConverter)
                .HasColumnType("json");

            // Salvar Comments (lista de mensagens) como JSON também
            entity.Property(a => a.Comments)
                .HasConversion(messageListConverter)
                .HasColumnType("json");
        });

        // Message
        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.Text).IsRequired();
        });

        base.OnModelCreating(modelBuilder);
    }
}