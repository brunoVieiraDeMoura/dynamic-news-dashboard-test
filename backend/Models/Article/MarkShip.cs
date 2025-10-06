using System.ComponentModel.DataAnnotations.Schema;

namespace jornal.Models.Article;
[NotMapped]
public class MarkShip
{
    public string Type { get; set; } = string.Empty;
    public Attributes? Attrs { get; set; }
}
