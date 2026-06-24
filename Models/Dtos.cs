using System.ComponentModel.DataAnnotations;

namespace SafeVault.Web.Models;


public class RegisterRequest
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
 
    [RegularExpression(@"^[a-zA-Z0-9_.-]+$",
        ErrorMessage = "Username may only contain letters, numbers, '.', '_' and '-'.")]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 8)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
        ErrorMessage = "Password must contain upper, lower, digit, and special character.")]
    public string Password { get; set; } = string.Empty;

    [StringLength(100)]
    public string DisplayName { get; set; } = string.Empty;
}

public class LoginRequest
{
    [Required]
    [StringLength(256)]
    public string UsernameOrEmail { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Password { get; set; } = string.Empty;
}

public class SearchRequest
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    [RegularExpression(@"^[a-zA-Z0-9 ]+$",
        ErrorMessage = "Search term may only contain letters, numbers, and spaces.")]
    public string Term { get; set; } = string.Empty;
}
