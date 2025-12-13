using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Model.Departments;
using TaskManager.Domain.Queries;
using TaskManager.Persistence.Data;

namespace TaskManager.Persistence.Queries;

public class DepartmentQuery(TaskManagerDbContext context) : IDepartmentQuery
{
    public async Task<List<DepartmentSelectModel>> GetAllAsync()
    {
        var departments = await context.Employees.Select(employee => employee.Department)
                                                 .Distinct()
                                                 .Select(department => new DepartmentSelectModel
                                                 {
                                                    NameDepartment = department
                                                 })
                                                 .OrderBy(department => department.NameDepartment)
                                                 .ToListAsync();

        return departments;
    }

    public Task<string?> GetByIdEmployeeAsync(int idCurrentUser)
    {
        var nameDepartment = context.Employees.Where(employee => employee.Id == idCurrentUser)
                                              .Select(employee => employee.Department)
                                              .FirstOrDefaultAsync();

        return nameDepartment;
    }
}