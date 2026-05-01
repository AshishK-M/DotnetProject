using System.ComponentModel.DataAnnotations;

namespace MyMvcApp.Models;

public class User
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string Role { get; set; } = "User";
}

public static class Roles
{
    public const string Admin = "Admin";
    public const string User = "User";
}
