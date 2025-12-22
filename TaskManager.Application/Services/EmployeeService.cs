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
/// Обеспечивает бизнес-логику работы с сотрудниками, включая валидацию данных,
/// проверку уникальности логинов, обработку строковых данных и взаимодействие с репозиториями.
/// </summary>
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
    /// Задача, результат которой содержит модель <see cref="EmployeeFotEditModel"/> 
    /// или <c>null</c>, если сотрудник не найден.
    /// </returns>
    /// <remarks>
    /// Запрос возвращает все поля сотрудника, необходимые для формы редактирования.
    /// </remarks>
    public async Task<EmployeeFotEditModel?> GetEmployeeForEditAsync(int id)
    {
        var employee = await _employeeQuery.GetEmployeeForEditAsync(id);
        
        return employee;
    }

    /// <summary>
    /// Создает нового сотрудника.
    /// </summary>
    /// <param name="employee">Сотрудник для создания.</param>
    /// <returns>Задача, представляющая асинхронную операцию создания.</returns>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если <paramref name="employee"/> равен <c>null</c>.
    /// </exception>
    /// <exception cref="LoginAlreadyExistsException">
    /// Выбрасывается, если логин сотрудника уже существует в системе.
    /// </exception>
    /// <remarks>
    /// <para>При создании сотрудника выполняется:</para>
    /// <para>1. Проверка уникальности логина</para>
    /// <para>2. Очистка строковых полей от лишних пробелов</para>
    /// <para>3. Нормализация отдела (удаление лишних пробелов)</para>
    /// <para>4. Преобразование пробелов в логине в подчеркивания</para>
    /// <para>5. Установка пароля по умолчанию</para>
    /// <para>6. Установка роли "Сотрудник"</para>
    /// <para>7. Сохранение в базу данных</para>
    /// </remarks>
    public async Task CreateAsync(Employee employee)
    {
        ArgumentNullException.ThrowIfNull(employee);

        // Проверка уникальности логина
        var loginExists = await _employeeRepository.IsLoginExistAsync(employee.Login);
        if (loginExists)
        {
            throw new LoginAlreadyExistsException(employee.Login);
        }

        // Очистка и нормализация данных
        TrimEmployeeStrings(employee);
        employee.Department = EmployeeStringProcessor.CleanSpaces(employee.Department);
        employee.Login = EmployeeStringProcessor.ConvertSpacesToUnderscore(employee.Login);

        // Установка значений по умолчанию
        employee.Password = DefaultPassword;
        employee.Role = UserRole.Employee;

        await _employeeRepository.AddAsync(employee);
    }

    /// <summary>
    /// Редактирует существующего сотрудника.
    /// </summary>
    /// <param name="employee">Сотрудник с обновленными данными.</param>
    /// <returns>Задача, представляющая асинхронную операцию редактирования.</returns>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если <paramref name="employee"/> равен <c>null</c>.
    /// </exception>
    /// <exception cref="LoginAlreadyExistsException">
    /// Выбрасывается, если обновленный логин уже существует у другого сотрудника.
    /// </exception>
    /// <remarks>
    /// <para>При редактировании сотрудника выполняется:</para>
    /// <para>1. Проверка уникальности логина (исключая текущего сотрудника)</para>
    /// <para>2. Очистка строковых полей от лишних пробелов</para>
    /// <para>3. Обновление в базе данных</para>
    /// <para>Примечание: пароль и роль не изменяются этим методом.</para>
    /// </remarks>
    public async Task EditAsync(Employee employee)
    {
        ArgumentNullException.ThrowIfNull(employee);

        // Проверка уникальности логина (исключая текущего сотрудника)
        var loginExists = await _employeeRepository.IsLoginExistAsync(employee.Login, employee.Id);
        if (loginExists)
        {
            throw new LoginAlreadyExistsException(employee.Login);
        }

        // Очистка данных
        //TrimEmployeeStrings(employee);

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