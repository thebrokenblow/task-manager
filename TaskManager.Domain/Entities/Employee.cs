using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities;

/// <summary>
/// Представляет сотрудника в системе управления задачами.
/// Содержит основные данные о сотруднике для назначения и учета задач.
/// </summary>
public sealed class Employee
{
    /// <summary>
    /// Уникальный идентификатор сотрудника.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Полное имя сотрудника (фамилия и инициалы).
    /// Обязательное свойство.
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Подразделение или отдел, в котором работает сотрудник.
    /// Обязательное свойство.
    /// </summary>
    public required string Department { get; set; }

    /// <summary>
    /// Логин сотрудника.
    /// Обязательное свойство.
    /// </summary>
    public required string Login { get; set; }

    /// <summary>
    /// Пароль сотрудника.
    /// Обязательное свойство.
    /// </summary>
    public required string Password { get; set; }

    /// <summary>
    /// Тип сотрудника.
    /// Обязательное свойство.
    /// </summary>
    public required UserRole Role { get; set; }
}