using TaskManager.Application.Services.Interfaces;
using TaskManager.Domain.Model.Departments;
using TaskManager.Domain.Queries;

namespace TaskManager.Application.Services;

/// <summary>
/// Сервис для работы с подразделениями.
/// </summary>
public class DepartmentService(IDepartmentQuery departmentQuery) : IDepartmentService
{
    /// <summary>
    /// Получает все подразделения.
    /// </summary>
    public async Task<IEnumerable<DepartmentSelectModel>> GetAllAsync()
    {
        var departments = await departmentQuery.GetDepartmentsAsync();

        return departments;
    }
}