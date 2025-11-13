using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using TaskManager.Repositories.Interfaces;
using TaskManager.Services.Interfaces;
using TaskManager.ViewModel;

namespace TaskManager.Services;

public class AuthService(
    IUserRepository userRepository, 
    IHttpContextAccessor httpContextAccessor) : IAuthService
{
    public const string AdminLogin = "lunnen";

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    public string? CurrentUserLogin =>
        _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;
    
    public bool IsAdmin =>
        CurrentUserLogin == AdminLogin;

    public int? CurrentUserId
    {
        get
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim is not null)
            {
                return int.Parse(userIdClaim.Value);
            }

            return null;
        }
    }

    private readonly IUserRepository _userRepository = userRepository ??
            throw new ArgumentNullException(nameof(userRepository), "userRepository is null");

    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor ??
            throw new ArgumentNullException(nameof(httpContextAccessor), "httpContextAccessor is null");

    public async Task<bool> LoginAsync(LoginViewModel loginViewModel)
    {
        var user = await _userRepository.GetByLoginAsync(loginViewModel.Login);

        if (user == null)
        {
            return false;
        }

        if (user.Password != loginViewModel.Password)
        {
            return false;
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Login),
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30)
        };

        await _httpContextAccessor.HttpContext!.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

        return true;
    }

    public async Task LogoutAsync()
    {
        await _httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}