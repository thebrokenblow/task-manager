namespace TaskManager.Domain.Model.Employees.Query;

/// <summary>
/// Модель для отображения информации о сотрудниках в системе.
/// </summary>
public sealed class EmployeeForOverviewModel
{
    /// <summary>
    /// Уникальный идентификатор сотрудника.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Полное имя сотрудника (фамилия и инициалы).
    /// Обязательное свойство.
    /// </summary>
    public required string FullName { get; init; }

    /// <summary>
    /// Подразделение или отдел, в котором работает сотрудник.
    /// Обязательное свойство.
    /// </summary>
    public required string Department { get; init; }
}