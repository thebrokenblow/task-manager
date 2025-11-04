using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using TaskManager.Repositories.Interfaces;

namespace TaskManager.Services;

public class AuthService(
    IUserRepository userRepository,
    IHttpContextAccessor httpContextAccessor)
{
    public async Task<bool> LoginAsync(string password)
    {
        var user = await userRepository.GetByPasswordAsync(password);

        if (user == null)
        {
            return false;
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30)
        };

        await httpContextAccessor.HttpContext!.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

        return true;
    }

    public async Task LogoutAsync()
    {
        await httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public bool IsAuthenticated()
    {
        return httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
    }

    public bool IsInRole(string role)
    {
        return httpContextAccessor.HttpContext?.User.IsInRole(role) ?? false;
    }
}