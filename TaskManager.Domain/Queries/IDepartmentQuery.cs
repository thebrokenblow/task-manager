using TaskManager.Domain.Model.Departments.Query;

namespace TaskManager.Domain.Queries;

/// <summary>
/// Предоставляет запросы для работы с данными подразделений.
/// Реализует сценарии чтения данных.
/// </summary>
public interface IDepartmentQuery
{
    /// <summary>
    /// Получает все подразделения из системы.
    /// </summary>
    /// <returns>
    /// Задача, результат которой содержит перечисление моделей <see cref="DepartmentSelectModel"/>,
    /// </returns>
    Task<IEnumerable<DepartmentSelectModel>> GetDepartmentsAsync();

    /// <summary>
    /// Получает подразделение по идентификатору сотрудника.
    /// </summary>
    /// <param name="employeeId">Идентификатор сотрудника.</param>
    /// <returns>
    /// Задача, результат которой содержит модель <see cref="DepartmentModel"/> 
    /// с названием подразделения сотрудника или <c>null</c>, если сотрудник не найден.
    /// </returns>
    Task<DepartmentModel?> GetDepartmentByEmployeeIdAsync(int employeeId);
}