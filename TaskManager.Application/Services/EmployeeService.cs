using TaskManager.Application.Exceptions;
using TaskManager.Application.Services.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Model.Employees;
using TaskManager.Domain.Queries;
using TaskManager.Domain.Repositories;

namespace TaskManager.Application.Services;

/// <summary>
/// Реализация сервиса для управления сотрудниками.
/// </summary>
/// <remarks>
/// Обеспечивает бизнес-логику работы с сотрудниками, включая валидацию данных,
/// проверку уникальности логинов и взаимодействие с репозиториями.
/// </remarks>
/// <param name="employeeQuery">Запросы для получения данных о сотрудниках.</param>
/// <param name="employeeRepository">Репозиторий для работы с данными сотрудников.</param>
public class EmployeeService(
    IEmployeeQuery employeeQuery,
    IEmployeeRepository employeeRepository) : IEmployeeService
{
    /// <summary>
    /// Пароль по умолчанию для новых сотрудников.
    /// </summary>
    private const string DefaultPassword = "Qwerty123";

    /// <inheritdoc/>
    public async Task<List<Employee>> GetRegularEmployeesAsync()
    {
        var employees = await employeeQuery.GetRegularEmployeesAsync();
        return employees;
    }

    /// <inheritdoc/>
    public async Task<List<EmployeeSelectModel>> GetResponsibleEmployeesAsync()
    {
        var responsibleEmployees = await employeeQuery.GetResponsibleEmployeesAsync();
        return responsibleEmployees;
    }

    /// <inheritdoc/>
    public async Task<Employee?> GetByIdAsync(int id)
    {
        var employee = await employeeRepository.GetByIdAsync(id);
        return employee;
    }

    /// <inheritdoc/>
    public async Task CreateAsync(Employee employee)
    {
        var loginExists = await employeeRepository.IsLoginExistAsync(employee.Login);

        if (loginExists)
        {
            throw new LoginAlreadyExistsException(employee.Login);
        }

        TrimEmployeeStrings(employee);

        employee.Password = DefaultPassword;
        employee.Role = UserRole.Employee;

        await employeeRepository.AddAsync(employee);
    }

    /// <inheritdoc/>
    public async Task EditAsync(Employee employee)
    {
        var loginExists = await employeeRepository.IsLoginExistAsync(employee.Login, employee.Id);

        if (loginExists)
        {
            throw new LoginAlreadyExistsException(employee.Login);
        }

        TrimEmployeeStrings(employee);
        await employeeRepository.UpdateAsync(employee);
    }

    /// <summary>
    /// Очищает строковые свойства сотрудника от лишних пробелов.
    /// </summary>
    /// <param name="employee">Сотрудник для обработки.</param>
    private static void TrimEmployeeStrings(Employee employee)
    {
        if (employee.FullName is not null)
        {
            employee.FullName = employee.FullName.Trim();
        }

        if (employee.Department is not null)
        {
            employee.Department = employee.Department.Trim();
        }

        if (employee.Login is not null)
        {
            employee.Login = employee.Login.Trim();
        }

        if (employee.Password is not null)
        {
            employee.Password = employee.Password.Trim();
        }
    }
}