using System.Text.Json.Serialization;

namespace jornal.Models;

public class Post
{
    public int Id { get; set; }
    [JsonIgnore]
    public User User { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }


}
public class PostDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public string UserName { get; set; }
}
