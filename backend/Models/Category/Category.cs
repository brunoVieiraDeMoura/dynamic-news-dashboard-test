namespace jornal.Models.Category;
public class Category
{
    public int Id { get; set; }
    public string Slug { get; set; }
    public string Name { get; set; }
    public ICollection<SubCategory>? SubCategories { get; set; } = new List<SubCategory>();
    public DateTime Date { get; set; } = DateTime.Now;
}
public class CategoryDto
{
    public int Id { get; set; }
    public string Slug { get; set; }
    public string Name { get; set; }
    public ICollection<SubCategory>? SubCategories { get; set; }
    public DateTime Date { get; set; }

}