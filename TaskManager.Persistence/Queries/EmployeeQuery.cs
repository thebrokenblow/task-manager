using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Model.Employees;
using TaskManager.Domain.Queries;
using TaskManager.Persistence.Data;

namespace TaskManager.Persistence.Queries;

public class EmployeeQuery(TaskManagerDbContext context) : IEmployeeQuery
{
    public async Task<List<Employee>> GetRegularEmployeesAsync()
    {
        var employees = await context.Employees.Where(employee => employee.Role != UserRole.Admin)
                                               .OrderBy(employee => employee.Department)
                                               .ThenBy(employee => employee.FullName)
                                               .ToListAsync();

        return employees;
    }

    public async Task<List<EmployeeSelectModel>> GetResponsibleEmployeesAsync()
    {
        var employees = await context.Employees.Where(employee => employee.Role != UserRole.Admin)
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