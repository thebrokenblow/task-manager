using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Model.Employees;
using TaskManager.Domain.Queries;
using TaskManager.Persistence.Data;

namespace TaskManager.Persistence.Queries;

/// <summary>
/// Запросы для работы с сотрудниками.
/// </summary>
public class EmployeeQuery(TaskManagerDbContext context) : IEmployeeQuery
{
    /// <summary>
    /// Получает список обычных сотрудников.
    /// </summary>
    public async Task<List<Employee>> GetRegularEmployeesAsync()
    {
        var employees = await context.Employees.Where(employee => employee.Role != UserRole.Admin)
                                               .OrderBy(employee => employee.Department)
                                               .ThenBy(employee => employee.FullName)
                                               .ToListAsync();

        return employees;
    }

    /// <summary>
    /// Получает список ответственных сотрудников отдела.
    /// </summary>
    /// <param name="department">Название отдела.</param>
    public async Task<List<EmployeeSelectModel>> GetResponsibleEmployeesAsync(string department)
    {
        var employees = await context.Employees.Where(employee => employee.Role != UserRole.Admin && employee.Department == department)
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
}