using TaskManager.Domain.Entities;

namespace TaskManager.Application.Services.Interfaces;

public interface IEmployeeService
{
    Task<List<Employee>> GetAllAsync();
    Task EditAsync(Employee employee);
    Task CreateAsync(Employee employee);
}