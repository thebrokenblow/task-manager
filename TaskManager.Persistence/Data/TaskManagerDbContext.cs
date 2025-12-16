using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TaskManager.Domain.Entities;

namespace TaskManager.Persistence.Data;

/// <summary>
/// Контекст базы данных приложения TaskManager.
/// </summary>
public class TaskManagerDbContext(DbContextOptions<TaskManagerDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Набор данных документов. Используется для операций с таблицей документов.
    /// </summary>
    public DbSet<Document> Documents { get; set; }

    /// <summary>
    /// Набор данных сотрудников. Используется для операций с таблицей сотрудников.
    /// </summary>
    public DbSet<Employee> Employees { get; set; }

    /// <summary>
    /// Настраивает модель данных при создании контекста.
    /// Загружает конфигурации сущностей из текущей сборки.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = Assembly.GetExecutingAssembly();
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        base.OnModelCreating(modelBuilder);
    }
}