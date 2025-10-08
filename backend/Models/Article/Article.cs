namespace jornal.Models.Article;
public class Article
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public int SubCategoryId { get; set; }
    public string Category { get; set; }
    public string SubCategory { get; set; }
    public bool Accepted { get; set; }
    public int? WriterID { get; set; }
    public int? ReviewID { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public Document Post { get; set; } = new();
    public DateTime Data { get; set; } = DateTime.Now;
    public List<Message>? Comments { get; set; }
}
public class ArticleDto
{
    public int CategoryId { get; set; }
    public int SubCategoryId { get; set; }
    public string Category { get; set; }
    public string SubCategory { get; set; }
    public bool Accepted { get; set; }
    public int? WriterID { get; set; }
    public int? ReviewID { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public DocumentDto Post { get; set; } = new();
    public DateTime Data { get; set; }
    public List<MessageDto>? Comments { get; set; } = new();
}
public class ArticleCreate
{
    public int CategoryId { get; set; }
    public int SubCategoryId { get; set; }
    public bool Accepted { get; set; }
    public int? WriterID { get; set; }
    public int? ReviewID { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public Document Post { get; set; } = new();
}