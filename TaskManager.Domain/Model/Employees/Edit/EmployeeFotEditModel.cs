namespace TaskManager.Domain.Model.Employees;

public class EmployeeFotEditModel
{
    /// <summary>
    /// Уникальный идентификатор сотрудника
    /// </summary>
    public required int Id { get; set; }

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
