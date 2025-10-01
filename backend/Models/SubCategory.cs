namespace jornal.Models;
public class SubCategory
{
    public int Id { get; set; }
    public Category Category { get; set; }
    public int CategoryId { get; set; }
    public string Slug { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
}
public class SubCategoryDto
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string Slug { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
}