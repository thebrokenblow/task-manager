namespace TaskManager.Models;

/// <summary>
/// Представляет сотрудника в системе управления задачами
/// Содержит основные данные о сотруднике для назначения и учета задач
/// </summary>
public class Employee
{
    /// <summary>
    /// Уникальный идентификатор сотрудника
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Полное имя сотрудника (фамилия и инициалы)
    /// Обязательное поле
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Подразделение или отдел, в котором работает сотрудник
    /// Обязательное поле
    /// </summary>
    public required string Department { get; set; }

    /// <summary>
    /// Логин сотрудника
    /// </summary>
    public required string Login { get; set; }

    /// <summary>
    /// Пароль сотрудника
    /// </summary>
    public required string Password { get; set; }

    /// <summary>
    /// Тип сотрудника
    /// </summary>
    public required RolesDictionary Role { get; set; }
}