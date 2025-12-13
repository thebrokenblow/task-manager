using TaskManager.Domain.Model.Departments;

namespace TaskManager.Domain.Queries;

public interface IDepartmentQuery
{
    Task<List<DepartmentSelectModel>> GetAllAsync();
    Task<string?> GetByIdEmployeeAsync(int idCurrentUser);
}