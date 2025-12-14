using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Model.Departments;
using TaskManager.Domain.Queries;
using TaskManager.Persistence.Data;

namespace TaskManager.Persistence.Queries;

/// <summary>
/// Запросы для работы с подразделениями.
/// </summary>
public class DepartmentQuery(TaskManagerDbContext context) : IDepartmentQuery
{
    /// <summary>
    /// Получает все подразделения.
    /// </summary>
    public async Task<List<DepartmentSelectModel>> GetAllAsync()
    {
        var departments = await context.Employees.Select(employee => employee.Department)
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
    public Task<DepartmentModel?> GetDepartmentByEmployeeIdAsync(int employeeId)
    {
        var nameDepartment = context.Employees.Where(employee => employee.Id == employeeId)
                                              .Select(employee => new DepartmentModel
                                              {
                                                  Name = employee.Department
                                              })
                                              .FirstOrDefaultAsync();

        return nameDepartment;
    }
}