using Microsoft.AspNetCore.Authentication.Cookies;

namespace TaskManager.View.Extensions;

public static class AuthenticationConfigurationExtension
{
    private const int CookieExpireDays = 30;
    private const string LoginPath = "/Account/Login";
    private const string CookieName = "TaskManagerAuth";

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