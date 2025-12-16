using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Repositories;

/// <summary>
/// Репозиторий для работы с сущностью <see cref="Employee"/>.
/// Предоставляет методы для доступа и управления данными сотрудников.
/// </summary>
public interface IEmployeeRepository
{
    /// <summary>
    /// Получает сотрудника по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сотрудника.</param>
    /// <returns>
    /// Задача, результат которой содержит сотрудника или <c>null</c>, 
    /// если сотрудник с указанным идентификатором не найден.
    /// </returns>
    Task<Employee?> GetByIdAsync(int id);

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
    /// Обновляет данные существующего сотрудника.
    /// </summary>
    /// <param name="employee">Сотрудник с обновленными данными.</param>
    /// <returns>Задача, представляющая асинхронную операцию обновления.</returns>
    Task UpdateAsync(Employee employee);
}