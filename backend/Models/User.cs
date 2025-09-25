using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace jornal.Models;

public class User
{
    public int Id { get; set; }
    public  string Name { get; set; }
    public  string Email { get; set; }
    public  string Password { get; set; }
    public string Role { get; set; } = "user";
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public DateTime Date { get; set; } = DateTime.Now;
}
public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } = "user";
    public ICollection<Post> Posts { get; set; }
    public DateTime Date { get; set; }
}
public class UserCreateDto
{
    [Required(ErrorMessage = "Invalid user name")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Invalid user email")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Invalid user password")]
    public string Password { get; set; }
}