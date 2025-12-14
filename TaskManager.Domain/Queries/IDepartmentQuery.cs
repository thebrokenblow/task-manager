using TaskManager.Domain.Model.Departments;

namespace TaskManager.Domain.Queries;

/// <summary>
/// Интерфейс запросов для подразделений.
/// </summary>
public interface IDepartmentQuery
{
    /// <summary>
    /// Получает все подразделения.
    /// </summary>
    Task<List<DepartmentSelectModel>> GetAllAsync();

    /// <summary>
    /// Получает подразделение по идентификатору сотрудника.
    /// </summary>
    /// <param name="employeeId">Идентификатор сотрудника.</param>
    Task<DepartmentModel?> GetDepartmentByEmployeeIdAsync(int employeeId);
}