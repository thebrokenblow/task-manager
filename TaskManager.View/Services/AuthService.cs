using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Model.Employees;
using TaskManager.Domain.Repositories;
using TaskManager.Domain.Services;

namespace TaskManager.View.Services;

/// <summary>
/// Сервис для управления аутентификацией.
/// </summary>
public sealed class AuthService(
    IEmployeeRepository employeeRepository,
    IHttpContextAccessor httpContextAccessor) : IAuthService
{
    private readonly IEmployeeRepository _employeeRepository =
        employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));

    private readonly IHttpContextAccessor _httpContextAccessor =
        httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

    /// <summary>
    /// Количество дней действия cookie аутентификации.
    /// </summary>
    private const int CookieExpireDays = 30;

    /// <summary>
    /// Проверяет, аутентифицирован ли пользователь.
    /// </summary>
    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    /// <summary>
    /// Проверяет, является ли пользователь администратором.
    /// </summary>
    public bool IsAdmin =>
        _httpContextAccessor.HttpContext?.User.IsInRole(UserRole.Admin.ToString()) ?? false;

    /// <summary>
    /// Получает полное имя текущего пользователя.
    /// </summary>
    public string? FullName =>
       _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value.ToString();

    /// <summary>
    /// Идентификатор администратора.
    /// </summary>
    public int IdAdmin => 1;

    /// <summary>
    /// Идентификатор текущего пользователя.
    /// </summary>
    public int? IdCurrentUser
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

    /// <summary>
    /// Роль текущего пользователя.
    /// </summary>
    public UserRole? Role
    {
        get
        {
            var roleValue = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(roleValue))
            {
                return null;
            }

            if (Enum.TryParse<UserRole>(roleValue, true, out var role))
            {
                return role;
            }

            return null;
        }
    }

    /// <summary>
    /// Выполняет вход пользователя в систему.
    /// </summary>
    /// <param name="loginViewModel">Модель данных для входа.</param>
    public async Task<bool> LoginAsync(EmployeeLoginModel loginViewModel)
    {
        var user = await _employeeRepository.GetByLoginAsync(loginViewModel.Login);

        if (user == null || 
            user.Login != loginViewModel.Login || 
            user.Password != loginViewModel.Password)
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
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(CookieExpireDays)
        };

        await _httpContextAccessor.HttpContext!.SignInAsync(
               CookieAuthenticationDefaults.AuthenticationScheme,
               new ClaimsPrincipal(claimsIdentity),
               authProperties);

        return true;
    }

    /// <summary>
    /// Выполняет выход пользователя из системы.
    /// </summary>
    public async Task LogoutAsync()
    {
        await _httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}