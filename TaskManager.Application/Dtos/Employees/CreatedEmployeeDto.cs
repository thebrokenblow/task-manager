namespace TaskManager.Application.Dtos.Employees;

/// <summary>
/// DTO для создания нового сотрудника в системе.
/// Содержит данные, необходимые для регистрации сотрудника.
/// </summary>
public sealed class CreatedEmployeeDto
{
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
}