using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Model.Employees;
using TaskManager.Domain.Model.Employees.Edit;
using TaskManager.Domain.Queries;
using TaskManager.Persistence.Data;

namespace TaskManager.Persistence.Queries;

/// <summary>
/// Предоставляет запросы для работы с данными сотрудников.
/// Реализует сценарии чтения данных.
/// </summary>
public class EmployeeQuery(TaskManagerDbContext context) : IEmployeeQuery
{
    private readonly TaskManagerDbContext _context =
        context ?? throw new ArgumentNullException(nameof(context));

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
    public async Task<IEnumerable<EmployeeForOverviewModel>> GetRegularEmployeesAsync()
    {
        var employees = await _context.Employees
            .Where(employee => employee.Role != UserRole.Admin)
            .Select(employee => new EmployeeForOverviewModel
            {
                Id = employee.Id,
                FullName = employee.FullName,
                Department = employee.Department
            })
            .OrderBy(employee => employee.Department)
            .ThenBy(employee => employee.FullName)
            .ToListAsync();

        return employees;
    }

    /// <summary>
    /// Получает список сотрудников указанного отдела.
    /// </summary>
    /// <param name="department">Название отдела для фильтрации сотрудников.</param>
    /// <returns>
    /// Задача, результат которой содержит перечисление моделей <see cref="EmployeeSelectModel"/>
    /// сотрудников указанного отдела, отсортированных по отделу и полному имени.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Выбрасывается, если <paramref name="department"/> равен <c>null</c> или пустой строке.
    /// </exception>
    /// <remarks>
    /// Метод возвращает сотрудников с ролью отличной от <see cref="UserRole.Admin"/>
    /// и принадлежащих указанному отделу.
    /// </remarks>
    public async Task<IEnumerable<EmployeeSelectModel>> GetEmployeesByDepartmentAsync(string department)
    {
        if (string.IsNullOrWhiteSpace(department))
        {
            throw new ArgumentException("Название отдела не может быть пустым", nameof(department));
        }

        var employees = await _context.Employees
            .Where(employee => employee.Role != UserRole.Admin && employee.Department == department)
            .OrderBy(employee => employee.Department)
            .ThenBy(employee => employee.FullName)
            .Select(employee => new EmployeeSelectModel
            {
                Id = employee.Id,
                FullNameAndDepartment = $"{employee.FullName} ({employee.Department})"
            })
            .ToListAsync();

        return employees;
    }

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
    public async Task<EmployeeFotOverviewEditModel?> GetEmployeeForEditAsync(int id)
    {
        var employees = await _context.Employees
            .Where(employee => employee.Id == id)
            .Select(employee => new EmployeeFotOverviewEditModel
            {
                Id = employee.Id,
                FullName = employee.FullName,
                Department = employee.Department,
                Login = employee.Login,
            })
            .FirstOrDefaultAsync();

        return employees;
    }

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
    public async Task<string?> GetPasswordAsync(int id)
    {
        var password = await _context.Employees
            .Where(employee => employee.Id == id)
            .Select(x => x.Password)
            .FirstOrDefaultAsync();

        return password;
    }
}