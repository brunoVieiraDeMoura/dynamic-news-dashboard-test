using System.ComponentModel.DataAnnotations.Schema;


namespace jornal.Models.Article;
[NotMapped]
public class Message
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ArticleId { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
public class MessageDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ArticleId { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; }
}
public class MessageCreat
{
    public int UserId { get; set; }
    public int ArticleId { get; set; }
    public string Text { get; set; }
}
public class MessageUpt
{
    public string text { get; set; }
}