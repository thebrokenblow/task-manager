using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using TaskManager.Domain.Entities.Dictionaries;
using TaskManager.Domain.Interfaces.Repositories;
using TaskManager.Domain.Interfaces.Services;
using TaskManager.Domain.Models;

namespace TaskManager.View.Services;

public class AuthService(
    IEmployeeRepository employeeRepository,
    IHttpContextAccessor httpContextAccessor) : IAuthService
{
    public int AdminId => 1;

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    public bool IsAdmin =>
        _httpContextAccessor.HttpContext?.User.IsInRole(RolesDictionary.Admin.ToString()) ?? false;

    public string? FullName =>
       _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value.ToString();

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

    private readonly IEmployeeRepository _employeeRepository = employeeRepository ??
            throw new ArgumentNullException(nameof(employeeRepository), "userRepository is null");

    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor ??
            throw new ArgumentNullException(nameof(httpContextAccessor), "httpContextAccessor is null");

    public async Task<bool> LoginAsync(LoginModel loginViewModel)
    {
        var user = await _employeeRepository.GetByLoginAsync(loginViewModel.Login);

        if (user == null)
        {
            return false;
        }

        if (user.Login != loginViewModel.Login)
        {
            return false;
        }

        if (user.Password != loginViewModel.Password)
        {
            return false;
        }

        var claims = new List<Claim>
        {
            new (ClaimTypes.Role, user.Role.ToString()),
            new (ClaimTypes.Name, user.FullName),
            new (ClaimTypes.NameIdentifier, user.Id.ToString()),
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