using TaskManager.Domain.Enums;
using TaskManager.Domain.Model.Employees;
using TaskManager.Domain.Model.Employees.Edit;

namespace TaskManager.Domain.Queries;

/// <summary>
/// Предоставляет запросы для работы с данными сотрудников.
/// Реализует сценарии чтения данных.
/// </summary>
public interface IEmployeeQuery
{
    /// <summary>
    /// Получает список обычных сотрудников (не администраторов) для обзора.
    /// </summary>
    /// <returns>
    /// Задача, результат которой содержит перечисление моделей <see cref="EmployeeForOverviewModel"/>,
    /// отсортированных по отделу и полному имени.
    /// </returns>
    /// <remarks>
    /// Метод возвращает только сотрудников с ролью отличной от <see cref="UserRole.Admin"/>.
    /// </remarks>
    Task<IEnumerable<EmployeeForOverviewModel>> GetRegularEmployeesAsync();

    /// <summary>
    /// Получает список сотрудников указанного отдела.
    /// </summary>
    /// <param name="department">Название отдела для фильтрации сотрудников.</param>
    /// <returns>
    /// Задача, результат которой содержит перечисление моделей <see cref="EmployeeSelectModel"/>
    /// сотрудников указанного отдела, отсортированных по отделу и полному имени.
    /// </returns>
    /// <remarks>
    /// Метод возвращает сотрудников с ролью отличной от <see cref="UserRole.Admin"/>
    /// и принадлежащих указанному отделу.
    /// </remarks>
    Task<IEnumerable<EmployeeSelectModel>> GetEmployeesByDepartmentAsync(string department);

    /// <summary>
    /// Получает данные сотрудника для редактирования.
    /// </summary>
    /// <param name="id">Идентификатор сотрудника.</param>
    /// <returns>
    /// Задача, результат которой содержит модель <see cref="EmployeeFotOverviewEditModel"/> 
    /// или <c>null</c>, если сотрудник не найден.
    /// </returns>
    /// <remarks>
    /// Запрос возвращает все поля сотрудника, необходимые для формы редактирования.
    /// </remarks>
    Task<EmployeeFotOverviewEditModel?> GetEmployeeForEditAsync(int id);

    /// <summary>
    /// Получает пароль сотрудника по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сотрудника.</param>
    /// <returns>
    /// Задача, результат которой содержит строку с паролем сотрудника,
    /// или <c>null</c>, если сотрудник с указанным идентификатором не найден
    /// или пароль не установлен.
    /// </returns>
    /// <remarks>
    Task<string?> GetPasswordAsync(int id);
}