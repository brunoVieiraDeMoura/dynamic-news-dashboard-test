using System.ComponentModel.DataAnnotations.Schema;


namespace jornal.Models.Article;
[NotMapped]
public class Message
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
}
public class MessageDto
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
}