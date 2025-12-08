using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Model.Employees;
using TaskManager.Domain.Repositories;
using TaskManager.Domain.Services;

namespace TaskManager.View.Services;

public class AuthService(
    IEmployeeRepository employeeRepository,
    IHttpContextAccessor httpContextAccessor) : IAuthService
{
    public bool IsAuthenticated =>
        httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    public bool IsAdmin =>
        httpContextAccessor.HttpContext?.User.IsInRole(RolesDictionary.Admin.ToString()) ?? false;

    public string? FullName =>
       httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value.ToString();

    public int IdAdmin => 1;

    public int? IdCurrentUser
    {
        get
        {
            var userIdClaim = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim is not null)
            {
                return int.Parse(userIdClaim.Value);
            }

            return null;
        }
    }

    public async Task<bool> LoginAsync(EmployeeLoginModel loginViewModel)
    {
        var user = await employeeRepository.GetByLoginAsync(loginViewModel.Login);

        if (user == null || user.Login != loginViewModel.Login || user.Password != loginViewModel.Password)
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
}