using TaskManager.Models;

namespace TaskManager.Repositories.Interfaces;

/// <summary>
/// Предоставляет методы для работы с данными пользователей в хранилище.
/// </summary>
public interface IUserRepository
{

    /// <summary>
    /// Получает пользователя по паролю.
    /// </summary>
    /// <param name="password">Пароль пользователя.</param>
    /// <returns>Задача, результатом которой является сущность пользователя или null если не найден.</returns>
    Task<User?> GetByPasswordAsync(string password);
}