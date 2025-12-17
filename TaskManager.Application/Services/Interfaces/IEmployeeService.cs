using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Model.Employees;

namespace TaskManager.Application.Services.Interfaces;

/// <summary>
/// Реализация сервиса для управления сотрудниками.
/// Обеспечивает бизнес-логику работы с сотрудниками, включая валидацию данных,
/// проверку уникальности логинов, обработку строковых данных и взаимодействие с репозиториями.
/// </summary>
public interface IEmployeeService
{
    /// <summary>
    /// Получает список обычных сотрудников (не администраторов).
    /// </summary>
    /// <returns>
    /// Задача, результат которой содержит перечисление моделей <see cref="EmployeeForOverviewModel"/>,
    /// отсортированных по отделу и полному имени.
    /// </returns>
    /// <remarks>
    /// Возвращает только сотрудников с ролью отличной от <see cref="UserRole.Admin"/>.
    /// </remarks>
    Task<IEnumerable<EmployeeForOverviewModel>> GetRegularEmployeesAsync();

    /// <summary>
    /// Получает список ответственных сотрудников отдела текущего пользователя.
    /// </summary>
    /// <returns>
    /// Задача, результат которой содержит перечисление моделей <see cref="EmployeeSelectModel"/>
    /// сотрудников отдела текущего пользователя.
    /// </returns>
    Task<IEnumerable<EmployeeSelectModel>> GetResponsibleEmployeesAsync();

    /// <summary>
    /// Получает сотрудника по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сотрудника.</param>
    /// <returns>
    /// Задача, результат которой содержит сущность <see cref="Employee"/>
    /// или <c>null</c>, если сотрудник не найден.
    /// </returns>
    Task<Employee?> GetByIdAsync(int id);

    /// <summary>
    /// Создает нового сотрудника.
    /// </summary>
    /// <param name="employee">Сотрудник для создания.</param>
    /// <returns>Задача, представляющая асинхронную операцию создания.</returns>
    Task CreateAsync(Employee employee);

    /// <summary>
    /// Редактирует существующего сотрудника.
    /// </summary>
    /// <param name="employee">Сотрудник с обновленными данными.</param>
    /// <returns>Задача, представляющая асинхронную операцию редактирования.</returns>
    Task EditAsync(Employee employee);
}