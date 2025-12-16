using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Persistence.Data.Configurations;

/// <summary>
/// Конфигурация таблицы сотрудников.
/// </summary>
public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    /// <summary>
    /// Настраивает таблицу сотрудников.
    /// </summary>
    /// <param name="builder">Строитель конфигурации сущности Employee.</param>
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("employees",
            tableBuilder =>
                tableBuilder.HasComment("Таблица для хранения данных сотрудников системы TaskManager"));

        builder.HasKey(employee => employee.Id);

        builder.Property(employee => employee.Id)
            .HasColumnName("id")
            .UseIdentityByDefaultColumn()
            .HasComment("Уникальный идентификатор сотрудника");

        builder.Property(employee => employee.FullName)
            .HasColumnName("full_name")
            .HasComment("Полное имя сотрудника (фамилия и инициалы)")
            .IsRequired();

        builder.Property(employee => employee.Department)
            .HasColumnName("department")
            .HasComment("Подразделение или отдел, в котором работает сотрудник")
            .IsRequired();

        builder.Property(employee => employee.Login)
            .HasColumnName("login")
            .HasComment("Логин сотрудника для входа в систему")
            .IsRequired();

        builder.Property(employee => employee.Password)
            .HasColumnName("password")
            .HasComment("Пароль сотрудника для входа в систему")
            .IsRequired();

        builder.Property(employee => employee.Role)
            .HasColumnName("role")
            .HasComment("Тип сотрудника (роль в системе)")
            .IsRequired();
    }
}