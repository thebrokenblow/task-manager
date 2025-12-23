namespace TaskManager.Domain.Model.Departments.Query;

/// <summary>
/// Модель для выбора подразделения из списка.
/// </summary>
/// <remarks>
/// Используется для отображения краткой информации о подразделении 
/// при выборе из выпадающего списка или других UI-элементов.
/// Содержит только базовую информацию, необходимую для идентификации подразделения.
/// </remarks>
public sealed class DepartmentSelectModel
{
    /// <summary>
    /// Название подразделения.
    /// </summary>
    /// <value>Строка с наименованием подразделения.</value>
    /// <example>"Отдел разработки", "Бухгалтерия"</example>
    public required string Name { get; init; }
}