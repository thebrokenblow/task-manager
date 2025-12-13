using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Services;
using TaskManager.Application.Services.Interfaces;

namespace TaskManager.Application;

/// <summary>
/// Методы расширения для настройки зависимостей слоя Application.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Регистрирует сервисы слоя Application в контейнере зависимостей.
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IDocumentService, DocumentService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IDepartmentService, DepartmentService>();

        services.AddScoped<IExportService, ExportService>();

        return services;
    }
}