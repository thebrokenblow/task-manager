using TaskManager.Application.Dtos.Employees;
using TaskManager.Application.Exceptions;
using TaskManager.Application.Services.Interfaces;
using TaskManager.Application.Utilities;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Exceptions;
using TaskManager.Domain.Model.Employees.Edit;
using TaskManager.Domain.Model.Employees.Query;
using TaskManager.Domain.Queries;
using TaskManager.Domain.Repositories;
using TaskManager.Domain.Services;

namespace TaskManager.Application.Services;

/// <summary>
/// Реализация сервиса для управления сотрудниками.
/// Обеспечивает бизнес-логику работы с сотрудниками, включая валидацию данных,
/// проверку уникальности логинов, обработку строковых данных и взаимодействие с репозиториями.
/// </summary>
public sealed class EmployeeService(
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
    /// Получает список обычных сотрудников (не администраторов).
    /// </summary>
    /// <returns>
    /// Задача, результат которой содержит перечисление моделей <see cref="EmployeeForOverviewModel"/>,
    /// отсортированных по отделу и полному имени.
    /// </returns>
    /// <remarks>
    /// Возвращает только сотрудников с ролью отличной от <see cref="UserRole.Admin"/>.
    /// Результат сортируется сначала по отделу, затем по полному имени.
    /// </remarks>
    public async Task<IEnumerable<EmployeeForOverviewModel>> GetRegularEmployeesAsync()
    {
        var employees = await _employeeQuery.GetRegularEmployeesAsync();

        return employees;
    }

    /// <summary>
    /// Получает список ответственных сотрудников отдела текущего пользователя.
    /// </summary>
    /// <returns>
    /// Задача, результат которой содержит перечисление моделей <see cref="EmployeeSelectModel"/>
    /// сотрудников отдела текущего пользователя.
    /// </returns>
    /// <exception cref="UnauthorizedAccessException">
    /// Выбрасывается, если пользователь не аутентифицирован.
    /// </exception>
    /// <exception cref="NotFoundException">
    /// Выбрасывается, если у текущего пользователя не указан отдел.
    /// </exception>
    /// <remarks>
    /// <para>Метод выполняет следующие шаги:</para>
    /// <para>1. Проверяет аутентификацию текущего пользователя</para>
    /// <para>2. Получает отдел текущего пользователя</para>
    /// <para>3. Возвращает сотрудников этого отдела с ролью отличной от администратора</para>
    /// <para>Результат сортируется по отделу и полному имени.</para>
    /// </remarks>
    public async Task<IEnumerable<EmployeeSelectModel>> GetResponsibleEmployeesAsync()
    {
        if (!_authService.IsAuthenticated || !_authService.IdCurrentUser.HasValue)
        {
            throw new UnauthorizedAccessException("Пользователь не аутентифицирован");
        }

        var departmentModel = await _departmentQuery.GetDepartmentByEmployeeIdAsync(_authService.IdCurrentUser.Value) ??
            throw new NotFoundException("У пользователя не указан отдел", _authService.IdCurrentUser.Value);

        var responsibleEmployees = await _employeeQuery.GetEmployeesByDepartmentAsync(departmentModel.Name);

        return responsibleEmployees;
    }

    /// <summary>
    /// Получает данные сотрудника для редактирования.
    /// </summary>
    /// <param name="id">Идентификатор сотрудника.</param>
    /// <returns>
    /// Задача, результат которой содержит модель <see cref="EmployeeFotOverviewEditModel"/> 
    /// или <c>null</c>, если сотрудник не найден.
    /// </returns>
    /// <remarks>
    /// Запрос возвращает все поля сотрудника, необходимые для формы редактирования.
    /// </remarks>
    public async Task<EmployeeFotOverviewEditModel?> GetEmployeeForEditAsync(int id)
    {
        var employee = await _employeeQuery.GetEmployeeForEditAsync(id);
        
        return employee;
    }

    /// <summary>
    /// Создает нового сотрудника.
    /// </summary>
    /// <param name="createdEmployeeDto">Сотрудник для создания.</param>
    /// <returns>Задача, представляющая асинхронную операцию создания.</returns>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если <paramref name="createdEmployeeDto"/> равен <c>null</c>.
    /// </exception>
    /// <exception cref="LoginAlreadyExistsException">
    /// Выбрасывается, если логин сотрудника уже существует в системе.
    /// </exception>
    public async Task CreateAsync(CreatedEmployeeDto createdEmployeeDto)
    {
        ArgumentNullException.ThrowIfNull(createdEmployeeDto);

        // Проверка уникальности логина
        var loginExists = await _employeeRepository.IsLoginExistAsync(createdEmployeeDto.Login);
        if (loginExists)
        {
            throw new LoginAlreadyExistsException(createdEmployeeDto.Login);
        }

        var employee = new Employee
        {
            FullName = createdEmployeeDto.FullName,
            Department = createdEmployeeDto.Department,
            Login = createdEmployeeDto.Login,
            Password = DefaultPassword,
            Role = UserRole.Employee
        };

        TrimEmployeeStrings(employee);

        employee.Department = EmployeeStringProcessor.CleanSpaces(employee.Department);
        employee.Login = EmployeeStringProcessor.ConvertSpacesToUnderscore(employee.Login);

        await _employeeRepository.AddAsync(employee);
    }

    /// <summary>
    /// Редактирует существующего сотрудника.
    /// </summary>
    /// <param name="editedEmployeeDto">Сотрудник с обновленными данными.</param>
    /// <returns>Задача, представляющая асинхронную операцию редактирования.</returns>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если <paramref name="editedEmployeeDto"/> равен <c>null</c>.
    /// </exception>
    /// <exception cref="LoginAlreadyExistsException">
    /// Выбрасывается, если обновленный логин уже существует у другого сотрудника.
    /// </exception>
    public async Task EditAsync(EditedEmployeeDto editedEmployeeDto)
    {
        ArgumentNullException.ThrowIfNull(editedEmployeeDto);

        // Проверка уникальности логина (исключая текущего сотрудника)
        var loginExists = await _employeeRepository.IsLoginExistAsync(editedEmployeeDto.Login, editedEmployeeDto.Id);
        if (loginExists)
        {
            throw new LoginAlreadyExistsException(editedEmployeeDto.Login);
        }

        TrimEmployeeStrings(editedEmployeeDto);

        var employeeFotEditModel = new EmployeeFotEditModel
        {
            Id = editedEmployeeDto.Id,
            FullName = editedEmployeeDto.FullName ?? string.Empty,
            Department = editedEmployeeDto.Department ?? string.Empty,
            Login = editedEmployeeDto.Login ?? string.Empty,
        };

        await _employeeRepository.UpdateBasicInfoAsync(employeeFotEditModel);
    }

    /// <summary>
    /// Очищает строковые свойства сотрудника от лишних пробелов.
    /// </summary>
    /// <param name="editedEmployeeDto">Сотрудник для обработки.</param>
    private static void TrimEmployeeStrings(EditedEmployeeDto editedEmployeeDto)
    {
        if (editedEmployeeDto.FullName is not null)
        {
            editedEmployeeDto.FullName = editedEmployeeDto.FullName.Trim();
        }

        if (editedEmployeeDto.Department is not null)
        {
            editedEmployeeDto.Department = editedEmployeeDto.Department.Trim();
        }

        if (editedEmployeeDto.Login is not null)
        {
            editedEmployeeDto.Login = editedEmployeeDto.Login.Trim();
        }
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