using Microsoft.EntityFrameworkCore;
using TaskManager.Application;
using TaskManager.Domain.Services;
using TaskManager.Persistence;
using TaskManager.View.Extensions;
using TaskManager.View.Services;
using TaskManager.View.Utilities;

namespace TaskManager.View;

/// <summary>
/// Класс настройки приложения.
/// </summary>
public class Startup(IConfiguration configuration)
{
    /// <summary>
    /// Настраивает сервисы приложения.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddControllersWithViews();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<DocumentStyler>();

        services.ConfigureAuthentication();

        services.AddAuthorization();
        services.AddApplication();

        var connectionString = configuration.GetConnectionString("DbConnection") ??
            throw new InvalidOperationException("Connection string is not initialized");

        services.AddPersistence(connectionString);
    }

    /// <summary>
    /// Настраивает конвейер обработки HTTP-запросов.
    /// </summary>
    /// <param name="app">Построитель приложения.</param>
    /// <param name="env">Окружение веб-хостинга.</param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (!env.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseGlobalErrorHandling();

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Documents}/{action=Index}/{id?}");
        });
    }
}