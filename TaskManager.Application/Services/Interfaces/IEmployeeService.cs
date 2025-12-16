using TaskManager.Domain.Entities;
using TaskManager.Domain.Model.Employees;

namespace TaskManager.Application.Services.Interfaces;

/// <summary>
/// Сервис для управления сотрудниками.
/// </summary>
public interface IEmployeeService
{
    /// <summary>
    /// Получает список обычных сотрудников (не администраторов).
    /// </summary>
    /// <returns>Список сотрудников с ролью "Сотрудник".</returns>
    Task<IEnumerable<EmployeeForOverviewModel>> GetRegularEmployeesAsync();

    /// <summary>
    /// Получает список ответственных сотрудников для выбора в интерфейсе.
    /// </summary>
    /// <returns>Список моделей сотрудников для выпадающих списков.</returns>
    Task<IEnumerable<EmployeeSelectModel>> GetResponsibleEmployeesAsync();

    /// <summary>
    /// Получает сотрудника по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сотрудника.</param>
    /// <returns>Объект сотрудника или null, если сотрудник не найден.</returns>
    Task<Employee?> GetByIdAsync(int id);

    /// <summary>
    /// Создает нового сотрудника.
    /// </summary>
    /// <param name="employee">Объект сотрудника для создания.</param>
    Task CreateAsync(Employee employee);

    /// <summary>
    /// Редактирует существующего сотрудника.
    /// </summary>
    /// <param name="employee">Объект сотрудника с обновленными данными.</param>
    Task EditAsync(Employee employee);
}