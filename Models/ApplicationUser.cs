using Microsoft.AspNetCore.Identity;

namespace SafeVault.Web.Models;

/// <summary>
/// Extends ASP.NET Core Identity's built-in user so password hashing,
/// lockout, and security-stamp handling are all managed by the
/// Identity framework rather than custom (and error-prone) code.
/// </summary>
public class ApplicationUser : IdentityUser
{
    public string DisplayName { get; set; } = string.Empty;
}
