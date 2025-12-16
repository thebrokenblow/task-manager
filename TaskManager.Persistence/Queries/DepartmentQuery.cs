using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Model.Departments;
using TaskManager.Domain.Queries;
using TaskManager.Persistence.Data;

namespace TaskManager.Persistence.Queries;

/// <summary>
/// Предоставляет запросы для работы с данными подразделений.
/// Реализует сценарии чтения данных.
/// </summary>
public class DepartmentQuery(TaskManagerDbContext context) : IDepartmentQuery
{
    private readonly TaskManagerDbContext _context =
        context ?? throw new ArgumentNullException(nameof(context));

    /// <summary>
    /// Получает все подразделения из системы.
    /// </summary>
    /// <returns>
    /// Задача, результат которой содержит перечисление моделей <see cref="DepartmentSelectModel"/>,
    /// отсортированных по названию подразделения.
    /// </returns>
    public async Task<IEnumerable<DepartmentSelectModel>> GetDepartmentsAsync()
    {
        var departments = await _context.Employees
            .Select(employee => employee.Department)
            .Distinct()
            .Select(department => new DepartmentSelectModel
            {
                Name = department
            })
            .OrderBy(department => department.Name)
            .ToListAsync();

        return departments;
    }

    /// <summary>
    /// Получает подразделение по идентификатору сотрудника.
    /// </summary>
    /// <param name="employeeId">Идентификатор сотрудника.</param>
    /// <returns>
    /// Задача, результат которой содержит модель <see cref="DepartmentModel"/> 
    /// с названием подразделения сотрудника или <c>null</c>, если сотрудник не найден.
    /// </returns>
    public async Task<DepartmentModel?> GetDepartmentByEmployeeIdAsync(int employeeId)
    {
        var nameDepartment = await _context.Employees
            .Where(employee => employee.Id == employeeId)
            .Select(employee => new DepartmentModel
            {
                Name = employee.Department
            })
            .FirstOrDefaultAsync();

        return nameDepartment;
    }
}