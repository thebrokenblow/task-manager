using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Domain.Queries;
using TaskManager.Domain.Repositories;
using TaskManager.Persistence.Data;
using TaskManager.Persistence.Queries;
using TaskManager.Persistence.Repositories;

namespace TaskManager.Persistence;

/// <summary>
/// Расширение для регистрации зависимостей слоя Persistence.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Регистрирует сервисы слоя Persistence.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="connectionString">Строка подключения к базе данных.</param>
    public static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<TaskManagerDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IDocumentQuery, DocumentQuery>();
        services.AddScoped<IEmployeeQuery, EmployeeQuery>();
        services.AddScoped<IDepartmentQuery, DepartmentQuery>();

        services.AddScoped<IDocumentRepository, DocumentRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();

        return services;
    }
}