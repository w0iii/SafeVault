using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SafeVault.Web.Models;

namespace SafeVault.Web.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize] // baseline: any endpoint here requires a valid, authenticated JWT
public class AdminController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    // RBAC in action: only users carrying the "Admin" role claim can reach
    // this action. A "User"-role JWT receives 403 Forbidden, not 401 -
    // they ARE authenticated, they're just not authorized for this resource.
    [HttpGet("dashboard")]
    [Authorize(Roles = "Admin")]
    public IActionResult Dashboard()
    {
        return Ok(new { message = "Welcome, admin. This data is restricted to the Admin role." });
    }

    [HttpPost("promote/{userId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> PromoteToAdmin(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return NotFound();
        }

        if (!await _roleManager.RoleExistsAsync("Admin"))
        {
            await _roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        await _userManager.AddToRoleAsync(user, "Admin");
        return Ok(new { message = $"User {user.UserName} promoted to Admin." });
    }

    // Any authenticated user (User or Admin role) can reach this one -
    // shows the contrast between "must be logged in" and "must be a specific role".
    [HttpGet("profile-status")]
    public IActionResult ProfileStatus()
    {
        return Ok(new { message = "You are authenticated.", user = User.Identity?.Name });
    }
}
