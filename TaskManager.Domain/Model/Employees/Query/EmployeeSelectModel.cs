namespace TaskManager.Domain.Model.Employees.Query;

/// <summary>
/// Модель для отображения данных сотрудника в выпадающих списках и элементах выбора.
/// Содержит минимальный набор свойств, необходимых для идентификации и выбора сотрудника.
/// </summary>
public sealed class EmployeeSelectModel
{
    /// <summary>
    /// Уникальный идентификатор сотрудника.
    /// Используется для привязки выбранного значения в UI.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Форматированная строка, объединяющая полное имя сотрудника и отдел.
    /// Используется для отображения в элементах выбора в формате "ФИО (Отдел)".
    /// </summary>
    public required string FullNameAndDepartment { get; set; }
}