using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Domain.Queries;
using TaskManager.Domain.Repositories;
using TaskManager.Persistence.Data;
using TaskManager.Persistence.Queries;
using TaskManager.Persistence.Repositories;

namespace TaskManager.Persistence;

public static class DependencyInjection
{
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