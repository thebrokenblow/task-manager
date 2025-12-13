using TaskManager.Application.Services.Interfaces;
using TaskManager.Domain.Model.Departments;
using TaskManager.Domain.Queries;

namespace TaskManager.Application.Services;

public class DepartmentService(IDepartmentQuery departmentQuery) : IDepartmentService
{
    public async Task<List<DepartmentSelectModel>> GetAllAsync()
    {
        var departments = await departmentQuery.GetAllAsync();

        return departments;
    }
}