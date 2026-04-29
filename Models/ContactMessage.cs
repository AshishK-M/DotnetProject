using System.ComponentModel.DataAnnotations;

namespace MyMvcApp.Models;

public class ContactMessage
{
    public int Id { get; set; }
    [Required]
    [StringLength(80)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(2000, MinimumLength = 5)]
    public string Message { get; set; } = string.Empty;
}
