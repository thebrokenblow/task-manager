using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Exceptions;
using TaskManager.Domain.Model.Employees;
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
    public async Task AddAsync(Employee employee)
    {
        ArgumentNullException.ThrowIfNull(employee);

        await _context.AddAsync(employee);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Обновляет основные данные существующего сотрудника (ФИО, отдел и логин).
    /// Выполняет частичное обновление только разрешенных для изменения полей.
    /// </summary>
    /// <param name="editedEmployee">Модель с обновленными данными сотрудника.</param>
    /// <returns>Задача, представляющая асинхронную операцию обновления.</returns>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если <paramref name="editedEmployee"/> равен <c>null</c>.
    /// </exception>
    /// <exception cref="NotFoundException">
    /// Выбрасывается, если сотрудник с указанным <see cref="EmployeeFotEditModel.Id"/> не найден.
    /// </exception>
    /// <remarks>
    /// <para>Метод обновляет только следующие поля сотрудника:</para>
    /// <para>- <see cref="Employee.FullName"/> (полное имя)</para>
    /// <para>- <see cref="Employee.Department"/> (отдел)</para>
    /// <para>- <see cref="Employee.Login"/> (логин)</para>
    /// <para>Пароль, роль и другие системные поля не изменяются этим методом.</para>
    /// </remarks>
    public async Task UpdateBasicInfoAsync(EmployeeFotEditModel editedEmployee)
    {
        ArgumentNullException.ThrowIfNull(editedEmployee);

        var affectedRows = await _context.Employees
            .Where(employee => employee.Id == editedEmployee.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(employee => employee.FullName, editedEmployee.FullName)
                .SetProperty(employee => employee.Department, editedEmployee.Department)
                .SetProperty(employee => employee.Login, editedEmployee.Login));

        if (affectedRows == 0)
        {
            throw new NotFoundException(nameof(Employee), editedEmployee.Id);
        }
    }

    /// <summary>
    /// Обновляет пароль сотрудника.
    /// </summary>
    /// <param name="id">Идентификатор сотрудника, чей пароль нужно изменить.</param>
    /// <param name="password">Новый пароль сотрудника.</param>
    /// <returns>Задача, представляющая асинхронную операцию обновления пароля.</returns>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если <paramref name="password"/> равен <c>null</c>, пустой строке или состоит только из пробелов.
    /// </exception>
    /// <exception cref="NotFoundException">
    /// Выбрасывается, если сотрудник с указанным <paramref name="id"/> не найден.
    /// </exception>
    /// <remarks>
    /// <para>Метод выполняет прямое обновление пароля сотрудника в базе данных.</para>
    /// </remarks>
    public async Task UpdatePasswordAsync(int id, string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Пароль не может быть пустым", nameof(password));
        }

        var affectedRows = await _context.Employees
            .Where(employee => employee.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(employee => employee.Password, password));

        if (affectedRows == 0)
        {
            throw new NotFoundException(nameof(Employee), id);
        }
    }

    /// <summary>
    /// Проверяет существование сотрудника по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сотрудника для проверки.</param>
    /// <returns>
    /// Задача, результат которой содержит <c>true</c>, если сотрудник с указанным идентификатором существует;
    /// иначе <c>false</c>.
    /// </returns>
    public async Task<bool> IsExistAsync(int id)
    {
        var isExist = await _context.Employees.AnyAsync(employee => employee.Id == id);

        return isExist;
    }
}