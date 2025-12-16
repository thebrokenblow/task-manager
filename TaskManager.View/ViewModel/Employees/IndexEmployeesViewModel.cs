using TaskManager.Domain.Entities;
using TaskManager.Domain.Model.Employees;

namespace TaskManager.View.ViewModel.Employees;

/// <summary>
/// Модель представления для отображения списка сотрудников.
/// </summary>
public class IndexEmployeesViewModel
{
    /// <summary>
    /// Список сотрудников.
    /// </summary>
    public required IEnumerable<EmployeeForOverviewModel> Employees { get; init; }

    /// <summary>
    /// Сотрудник, которого не удалось создать.
    /// </summary>
    public Employee? FailedCreatedEmployee { get; set; }

    /// <summary>
    /// Текст сообщения о неудачном создании сотрудника.
    /// </summary>
    public string? TextFailedCreatedEmployee { get; set; }
}