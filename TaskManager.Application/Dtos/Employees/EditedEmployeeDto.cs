namespace TaskManager.Application.Dtos.Employees;

/// <summary>
/// DTO для редактирования данных сотрудника в системе.
/// Содержит идентификатор и обновляемые данные сотрудника.
/// </summary>
public sealed class EditedEmployeeDto
{
    /// <summary>
    /// Уникальный идентификатор сотрудника в системе.
    /// Обязательное свойство.
    /// </summary>
    public required int Id { get; init; }

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