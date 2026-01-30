using System.ComponentModel.DataAnnotations.Schema;

namespace jornal.Models.Article;
[NotMapped]
public class Attributes
{
    public string? Href { get; set; }
    public string? Target { get; set; }
    public string? Rel { get; set; }
    public string? Class { get; set; }
    public int? Level { get; set; }
    public string? Src { get; set; }
    public int? Start { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public string? Alt { get; set; }
    public string? Title { get; set; }
}
public class AttributesDto
{
    public string? Href { get; set; }
    public string? Target { get; set; }
    public string? Rel { get; set; }
    public string? Class { get; set; }
    public int? Level { get; set; }
    public string? Src { get; set; }
    public int? Start { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public string? Alt { get; set; }
    public string? Title { get; set; }
}
