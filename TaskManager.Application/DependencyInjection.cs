using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Services;
using TaskManager.Application.Services.Interfaces;

namespace TaskManager.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IDocumentService, DocumentService>();
        services.AddScoped<IEmployeeService, EmployeeService>();

        services.AddScoped<IExportService, ExportService>();

        return services;
    }
}