using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Repositories;
using TaskManager.Persistence.Data;

namespace TaskManager.Persistence.Repositories;

/// <summary>
/// Репозиторий для работы с сущностью <see cref="Employee"/>.
/// Предоставляет методы для доступа и управления данными сотрудников.
/// </summary>
public class EmployeeRepository(TaskManagerDbContext context) : IEmployeeRepository
{
    private readonly TaskManagerDbContext _context =
        context ?? throw new ArgumentNullException(nameof(context));

    /// <summary>
    /// Получает сотрудника по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сотрудника.</param>
    /// <returns>
    /// Задача, результат которой содержит сотрудника или <c>null</c>, 
    /// если сотрудник с указанным идентификатором не найден.
    /// </returns>
    public async Task<Employee?> GetByIdAsync(int id)
    {
        var employee = await _context.Employees.FindAsync(id);

        return employee;
    }

    /// <summary>
    /// Получает сотрудника по его логину.
    /// </summary>
    /// <param name="login">Логин сотрудника.</param>
    /// <returns>
    /// Задача, результат которой содержит сотрудника или <c>null</c>, 
    /// если сотрудник с указанным логином не найден.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Выбрасывается, если <paramref name="login"/> равен <c>null</c> или пустой строке.
    /// </exception>
    public async Task<Employee?> GetByLoginAsync(string login)
    {
        if (string.IsNullOrWhiteSpace(login))
        {
            throw new ArgumentException("Логин не может быть пустым", nameof(login));
        }

        var employee = await _context.Employees
            .FirstOrDefaultAsync(employee => employee.Login == login);

        return employee;
    }

    /// <summary>
    /// Проверяет, существует ли указанный логин в системе.
    /// </summary>
    /// <param name="login">Логин для проверки.</param>
    /// <param name="excludeEmployeeId">
    /// Идентификатор сотрудника, который исключается из проверки 
    /// </param>
    /// <returns>
    /// Задача, результат которой содержит <c>true</c>, если логин уже существует 
    /// (за исключением сотрудника с <paramref name="excludeEmployeeId"/>); 
    /// иначе <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Выбрасывается, если <paramref name="login"/> равен <c>null</c> или пустой строке.
    /// </exception>
    public async Task<bool> IsLoginExistAsync(string login, int? excludeEmployeeId = null)
    {
        if (string.IsNullOrWhiteSpace(login))
        {
            throw new ArgumentException("Логин не может быть пустым", nameof(login));
        }

        var isExist = await _context.Employees
            .AnyAsync(employee => employee.Login == login && employee.Id != excludeEmployeeId);

        return isExist;
    }

    /// <summary>
    /// Добавляет нового сотрудника в систему.
    /// </summary>
    /// <param name="employee">Сотрудник для добавления.</param>
    /// <returns>Задача, представляющая асинхронную операцию добавления.</returns>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если <paramref name="employee"/> равен <c>null</c>.
    /// </exception>
    /// <exception cref="DbUpdateException">
    /// Выбрасывается при возникновении ошибок при сохранении в базу данных.
    /// </exception>
    public async Task AddAsync(Employee employee)
    {
        ArgumentNullException.ThrowIfNull(employee);

        await _context.AddAsync(employee);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Обновляет данные существующего сотрудника.
    /// </summary>
    /// <param name="employee">Сотрудник с обновленными данными.</param>
    /// <returns>Задача, представляющая асинхронную операцию обновления.</returns>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если <paramref name="employee"/> равен <c>null</c>.
    /// </exception>
    /// <exception cref="DbUpdateException">
    /// Выбрасывается при возникновении ошибок при сохранении в базу данных.
    /// </exception>
    public async Task UpdateAsync(Employee employee)
    {
        ArgumentNullException.ThrowIfNull(employee);

        _context.Update(employee);
        await _context.SaveChangesAsync();
    }
}