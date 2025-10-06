using System.ComponentModel.DataAnnotations.Schema;

namespace jornal.Models.Article;
[NotMapped]
public class Node
{
    public string Type { get; set; }
    public Attributes? Attrs { get; set; }
    public List<Node>? Content { get; set; }
    public string? Text { get; set; }
    public List<MarkShip>? Marks { get; set; }
}
