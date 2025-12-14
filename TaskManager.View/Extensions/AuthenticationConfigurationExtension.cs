using Microsoft.AspNetCore.Authentication.Cookies;

namespace TaskManager.View.Extensions;

/// <summary>
/// Расширение для настройки аутентификации.
/// </summary>
public static class AuthenticationConfigurationExtension
{
    /// <summary>
    /// Количество дней действия cookie.
    /// </summary>
    private const int CookieExpireDays = 30;

    /// <summary>
    /// Путь к странице входа.
    /// </summary>
    private const string LoginPath = "/Account/Login";

    /// <summary>
    /// Имя cookie аутентификации.
    /// </summary>
    private const string CookieName = "TaskManagerAuth";

    /// <summary>
    /// Настраивает аутентификацию на основе cookies.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    public static IServiceCollection ConfigureAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = CookieName;
                options.LoginPath = LoginPath;
                options.ExpireTimeSpan = TimeSpan.FromDays(CookieExpireDays);
                options.SlidingExpiration = true;
            });

        return services;
    }
}