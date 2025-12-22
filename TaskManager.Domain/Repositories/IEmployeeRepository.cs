using TaskManager.Domain.Entities;
using TaskManager.Domain.Model.Employees;

namespace TaskManager.Domain.Repositories;

/// <summary>
/// Репозиторий для работы с сущностью <see cref="Employee"/>.
/// Предоставляет методы для доступа и управления данными сотрудников.
/// </summary>
public interface IEmployeeRepository
{
    /// <summary>
    /// Получает сотрудника по его логину.
    /// </summary>
    /// <param name="login">Логин сотрудника.</param>
    /// <returns>
    /// Задача, результат которой содержит сотрудника или <c>null</c>, 
    /// если сотрудник с указанным логином не найден.
    /// </returns>
    Task<Employee?> GetByLoginAsync(string login);

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
    Task<bool> IsLoginExistAsync(string login, int? excludeEmployeeId = null);

    /// <summary>
    /// Добавляет нового сотрудника в систему.
    /// </summary>
    /// <param name="employee">Сотрудник для добавления.</param>
    /// <returns>Задача, представляющая асинхронную операцию добавления.</returns>
    Task AddAsync(Employee employee);

    /// <summary>
    /// Обновляет основные данные существующего сотрудника (ФИО, отдел и логин).
    /// Выполняет частичное обновление только разрешенных для изменения полей.
    /// </summary>
    /// <param name="editedEmployee">Модель с обновленными данными сотрудника.</param>
    /// <returns>Задача, представляющая асинхронную операцию обновления.</returns>
    /// <remarks>
    /// <para>Метод обновляет только следующие поля сотрудника:</para>
    /// <para>- <see cref="Employee.FullName"/> (полное имя)</para>
    /// <para>- <see cref="Employee.Department"/> (отдел)</para>
    /// <para>- <see cref="Employee.Login"/> (логин)</para>
    /// <para>Пароль, роль и другие системные поля не изменяются этим методом.</para>
    /// </remarks>
    Task UpdateBasicInfoAsync(EmployeeFotEditModel editedEmployee);

    /// <summary>
    /// Обновляет пароль сотрудника.
    /// </summary>
    /// <param name="id">Идентификатор сотрудника, чей пароль нужно изменить.</param>
    /// <param name="password">Новый пароль сотрудника.</param>
    /// <returns>Задача, представляющая асинхронную операцию обновления пароля.</returns>
    Task UpdatePasswordAsync(int id, string password);

    /// <summary>
    /// Проверяет существование сотрудника по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сотрудника для проверки.</param>
    /// <returns>
    /// Задача, результат которой содержит <c>true</c>, если сотрудник с указанным идентификатором существует;
    /// иначе <c>false</c>.
    /// </returns>
    Task<bool> IsExistAsync(int id);
}