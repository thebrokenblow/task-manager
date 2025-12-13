using TaskManager.Domain.Model.Departments;

namespace TaskManager.Application.Services.Interfaces;

public interface IDepartmentService
{
    Task<List<DepartmentSelectModel>> GetAllAsync();
}