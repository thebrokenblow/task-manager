namespace TaskManager.Domain.Model.Employees.Edit;

/// <summary>
/// Модель сотрудника для отображения информации об сотруднике при редактировании.
/// </summary>
public sealed class EmployeeFotOverviewEditModel
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
}