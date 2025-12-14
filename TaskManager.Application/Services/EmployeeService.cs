using TaskManager.Application.Exceptions;
using TaskManager.Application.Services.Interfaces;
using TaskManager.Application.Utilities;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Exceptions;
using TaskManager.Domain.Model.Employees;
using TaskManager.Domain.Queries;
using TaskManager.Domain.Repositories;
using TaskManager.Domain.Services;

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
    IAuthService authService,
    IDepartmentQuery departmentQuery,
    IEmployeeQuery employeeQuery,
    IEmployeeRepository employeeRepository) : IEmployeeService
{
    private readonly IAuthService _authService = 
        authService ?? throw new ArgumentNullException(nameof(authService));

    private readonly IDepartmentQuery _departmentQuery = 
        departmentQuery ?? throw new ArgumentNullException(nameof(departmentQuery));

    private readonly IEmployeeQuery _employeeQuery = 
        employeeQuery ?? throw new ArgumentNullException(nameof(employeeQuery));

    private readonly IEmployeeRepository _employeeRepository = 
        employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));

    /// <summary>
    /// Пароль по умолчанию для новых сотрудников.
    /// </summary>
    private const string DefaultPassword = "Qwerty123";

    /// <summary>
    /// Получает список обычных сотрудников.
    /// </summary>
    public async Task<List<Employee>> GetRegularEmployeesAsync()
    {
        var employees = await _employeeQuery.GetRegularEmployeesAsync();
        return employees;
    }

    /// <summary>
    /// Получает список ответственных сотрудников.
    /// </summary>
    public async Task<List<EmployeeSelectModel>> GetResponsibleEmployeesAsync()
    {
        if (!_authService.IsAuthenticated || !_authService.IdCurrentUser.HasValue)
        {
            throw new UnauthorizedAccessException("Пользователь не аутентифицирован");
        }

        var departmentModel = await _departmentQuery.GetDepartmentByEmployeeIdAsync(_authService.IdCurrentUser.Value) ??
            throw new NotFoundException("У пользователя не указан отдел", _authService.IdCurrentUser.Value);

        var responsibleEmployees = await _employeeQuery.GetResponsibleEmployeesAsync(departmentModel.Name);

        return responsibleEmployees;
    }

    /// <summary>
    /// Получает сотрудника по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сотрудника.</param>
    public async Task<Employee?> GetByIdAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        return employee;
    }

    /// <summary>
    /// Создает нового сотрудника.
    /// </summary>
    /// <param name="employee">Сотрудник для создания.</param>
    public async Task CreateAsync(Employee employee)
    {
        var loginExists = await _employeeRepository.IsLoginExistAsync(employee.Login);

        if (loginExists)
        {
            throw new LoginAlreadyExistsException(employee.Login);
        }

        TrimEmployeeStrings(employee);

        employee.Department = EmployeeStringProcessor.CleanSpaces(employee.Department);
        employee.Login = EmployeeStringProcessor.ConvertSpacesToUnderscore(employee.Login);

        employee.Password = DefaultPassword;
        employee.Role = UserRole.Employee;

        await _employeeRepository.AddAsync(employee);
    }

    /// <summary>
    /// Редактирует существующего сотрудника.
    /// </summary>
    /// <param name="employee">Сотрудник с обновленными данными.</param>
    public async Task EditAsync(Employee employee)
    {
        var loginExists = await _employeeRepository.IsLoginExistAsync(employee.Login, employee.Id);

        if (loginExists)
        {
            throw new LoginAlreadyExistsException(employee.Login);
        }

        TrimEmployeeStrings(employee);
        await _employeeRepository.UpdateAsync(employee);
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