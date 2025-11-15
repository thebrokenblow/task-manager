using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Repositories.Interfaces;

namespace TaskManager.Repositories;

public class EmployeeRepository(TaskManagerDbContext context) : IEmployeeRepository
{
    public async Task<List<Employee>> GetAllAsync()
    {
        var employees = await context.Employees.ToListAsync();

        return employees;
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        var employee = await context.Employees.FirstOrDefaultAsync(employee => employee.Id == id);

        return employee;
    }

    public async Task<Employee?> GetByLoginAsync(string login)
    {
        var employee = await context.Employees.FirstOrDefaultAsync(employee => employee.Login == login);

        return employee;
    }

    public async Task AddAsync(Employee employee)
    {
        await context.AddAsync(employee);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Employee employee)
    {
        context.Update(employee);
        await context.SaveChangesAsync();
    }
}