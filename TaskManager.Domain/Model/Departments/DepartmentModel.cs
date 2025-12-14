namespace TaskManager.Domain.Model.Departments;

/// <summary>
/// Модель подразделения
/// </summary>
public class DepartmentModel
{
    /// <summary>
    /// Название подразделения.
    /// </summary>
    /// <value>Строка с наименованием подразделения.</value>
    /// <example>"Отдел разработки", "Бухгалтерия"</example>
    public required string Name { get; init; }
}