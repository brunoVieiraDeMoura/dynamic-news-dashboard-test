namespace jornal.Models.Article;
public class Article
{
    public int Id { get; set; }
    public int SubCategoryId { get; set; }
    public int? WriterID { get; set; }
    public int? ReviewID { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public Document Post { get; set; } = new();
    public DateTime Data { get; set; }
    public List<Message>? Comments { get; set; }
}