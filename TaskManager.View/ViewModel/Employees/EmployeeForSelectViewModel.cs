namespace TaskManager.View.ViewModel.Employees;

/// <summary>
/// Модель представления для выбора сотрудника.
/// </summary>
public class EmployeeForSelectViewModel
{
    /// <summary>
    /// Идентификатор сотрудника.
    /// </summary>
    public required int Id { get; init; }

    /// <summary>
    /// Полное имя сотрудника с указанием отдела.
    /// </summary>
    public required string FullNameAndDepartment { get; init; }
}