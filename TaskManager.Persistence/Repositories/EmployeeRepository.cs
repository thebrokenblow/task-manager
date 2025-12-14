using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Repositories;
using TaskManager.Persistence.Data;

namespace TaskManager.Persistence.Repositories;

/// <summary>
/// Репозиторий для работы с сотрудниками.
/// </summary>
public class EmployeeRepository(TaskManagerDbContext context) : IEmployeeRepository
{
    /// <summary>
    /// Получает сотрудника по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сотрудника.</param>
    public async Task<Employee?> GetByIdAsync(int id)
    {
        var employee = await context.Employees.FindAsync(id);

        return employee;
    }

    /// <summary>
    /// Получает сотрудника по логину.
    /// </summary>
    /// <param name="login">Логин сотрудника.</param>
    public async Task<Employee?> GetByLoginAsync(string login)
    {
        var employee = await context.Employees.FirstOrDefaultAsync(employee => employee.Login == login);

        return employee;
    }

    /// <summary>
    /// Проверяет существование логина.
    /// </summary>
    /// <param name="login">Логин для проверки.</param>
    /// <param name="excludeEmployeeId">Идентификатор сотрудника для исключения.</param>
    public async Task<bool> IsLoginExistAsync(string login, int? excludeEmployeeId = null)
    {
        var isExist = await context.Employees.AnyAsync(employee => employee.Login == login && employee.Id != excludeEmployeeId);

        return isExist;
    }

    /// <summary>
    /// Добавляет нового сотрудника.
    /// </summary>
    /// <param name="employee">Сотрудник для добавления.</param>
    public async Task AddAsync(Employee employee)
    {
        await context.AddAsync(employee);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Обновляет существующего сотрудника.
    /// </summary>
    /// <param name="employee">Сотрудник с обновленными данными.</param>
    public async Task UpdateAsync(Employee employee)
    {
        context.Update(employee);
        await context.SaveChangesAsync();
    }
}