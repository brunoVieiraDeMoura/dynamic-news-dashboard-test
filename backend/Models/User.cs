using System.Text.Json.Serialization;

namespace jornal.Models;

public class User
{
    public int Id { get; set; }
    public  string Name { get; set; }
    public  string Email { get; set; }
    public  string Password { get; set; }
    public string Role { get; set; } = "New";
    public ICollection<Post> Posts { get; set; } = new List<Post>();

}
public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } = "New";
    public ICollection<Post> Posts { get; set; }

}