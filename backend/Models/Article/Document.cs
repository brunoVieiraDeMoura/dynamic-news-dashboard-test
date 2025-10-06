using System.ComponentModel.DataAnnotations.Schema;

namespace jornal.Models.Article;
[NotMapped]
public class Document
{
    public string Type { get; set; } = "doc";
    public List<Node> Content { get; set; } = new();
}