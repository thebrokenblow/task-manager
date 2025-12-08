using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Repositories;

/// <summary>
/// Интерфейс для выполнения операций с сотрудниками в системе.
/// Определяет контракты для работы с хранилищем данных (базой данных).
/// </summary>
public interface IEmployeeRepository
{
    /// <summary>
    /// Получает сотрудника по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор сотрудника</param>
    /// <returns>Сотрудник или null, если сотрудник не найден</returns>
    Task<Employee?> GetByIdAsync(int id);

    /// <summary>
    /// Получает сотрудника по логину.
    /// </summary>
    /// <param name="login">Логин сотрудника</param>
    /// <returns>Сотрудник или null, если сотрудник не найден</returns>
    Task<Employee?> GetByLoginAsync(string login);

    /// <summary>
    /// Проверяет существование логина в системе.
    /// </summary>
    /// <param name="login">Логин для проверки</param>
    /// <param name="excludeEmployeeId">Идентификатор сотрудника, исключаемого из проверки</param>
    /// <returns>true, если логин существует, иначе false</returns>
    Task<bool> IsLoginExistAsync(string login, int? excludeEmployeeId = null);

    /// <summary>
    /// Добавляет нового сотрудника в хранилище.
    /// </summary>
    /// <param name="employee">Сотрудник для добавления</param>
    Task AddAsync(Employee employee);

    /// <summary>
    /// Обновляет существующего сотрудника в хранилище.
    /// </summary>
    /// <param name="employee">Сотрудник с обновленными данными</param>
    Task UpdateAsync(Employee employee);
}