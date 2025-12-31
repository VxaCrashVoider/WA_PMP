using System.ComponentModel.DataAnnotations;

namespace WA_ProjectManagement.Models;

public class RegisterRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = null!;

    [Required]
    public string DisplayName { get; set; } = null!;

    public string? AvatarUrl { get; set; }
}
