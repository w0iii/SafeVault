using System.ComponentModel.DataAnnotations;

namespace SafeVault.Web.Models;

/// <summary>
/// Input validation lives directly on the DTO via DataAnnotations so that
/// invalid input (malformed email, weak password, oversized/unsafe username)
/// is rejected by the model-binding pipeline before it ever reaches
/// application or data-access code.
/// </summary>
public class RegisterRequest
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    // Only allow safe characters: letters, numbers, underscore, dot, hyphen.
    // This blocks attempts to smuggle SQL/HTML/script payloads in via the username.
    [RegularExpression(@"^[a-zA-Z0-9_.-]+$",
        ErrorMessage = "Username may only contain letters, numbers, '.', '_' and '-'.")]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 8)]
    // Require upper, lower, digit, and special character.
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

/// <summary>DTO for the search demo endpoint used to illustrate SQL-injection prevention.</summary>
public class SearchRequest
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    // Deliberately restrictive: search terms only need word characters and spaces.
    // Anything else (quotes, semicolons, SQL keywords, angle brackets) is rejected outright.
    [RegularExpression(@"^[a-zA-Z0-9 ]+$",
        ErrorMessage = "Search term may only contain letters, numbers, and spaces.")]
    public string Term { get; set; } = string.Empty;
}
