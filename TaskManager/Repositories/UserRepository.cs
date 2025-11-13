using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Repositories.Interfaces;

namespace TaskManager.Repositories;

/// <summary>
/// Реализация репозитория для работы с данными пользователей
/// </summary>
public class UserRepository(TaskManagerDbContext context) : IUserRepository
{
    /// <summary>
    /// Получает пользователя по паролю
    /// </summary>
    /// <param name="password">Пароль пользователя</param>
    /// <returns>Задача, результатом которой является сущность пользователя или null если не найден</returns>
    public async Task<User?> GetByLoginAsync(string login)
    {
        var users = await context.Users.FirstOrDefaultAsync(user => user.Login == login);

        return users;
    }

    public async Task<int?> GetIdByLoginAsync(string login)
    {
        var user = await context.Users.Select(user => new { user.Id, user.Login })
                                      .FirstOrDefaultAsync(user => user.Login == login) 
        ?? throw new Exception();

        return user.Id;
    }
}