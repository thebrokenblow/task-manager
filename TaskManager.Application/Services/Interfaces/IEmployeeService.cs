using TaskManager.Domain.Entities;
using TaskManager.Domain.Model.Employees;

namespace TaskManager.Application.Services.Interfaces;

/// <summary>
/// Сервис для управления сотрудниками.
/// </summary>
/// <remarks>
/// Предоставляет методы для работы с сотрудниками, включая получение списков сотрудников,
/// создание, редактирование и управление данными сотрудников.
/// </remarks>
public interface IEmployeeService
{
    /// <summary>
    /// Получает список обычных сотрудников (не администраторов).
    /// </summary>
    /// <returns>Список сотрудников с ролью "Сотрудник".</returns>
    Task<List<Employee>> GetRegularEmployeesAsync();

    /// <summary>
    /// Получает список ответственных сотрудников для выбора в интерфейсе.
    /// </summary>
    /// <returns>Список моделей сотрудников для выпадающих списков.</returns>
    Task<List<EmployeeSelectModel>> GetResponsibleEmployeesAsync();

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
    /// <exception cref="LoginAlreadyExistsException">
    /// Выбрасывается, если логин сотрудника уже существует в системе.
    /// </exception>
    Task CreateAsync(Employee employee);

    /// <summary>
    /// Редактирует существующего сотрудника.
    /// </summary>
    /// <param name="employee">Объект сотрудника с обновленными данными.</param>
    /// <exception cref="LoginAlreadyExistsException">
    /// Выбрасывается, если обновленный логин сотрудника уже используется другим сотрудником.
    /// </exception>
    Task EditAsync(Employee employee);
}