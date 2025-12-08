using TaskManager.Domain.Model.Employees;

namespace TaskManager.Domain.Services;

/// <summary>
/// Интерфейс для службы аутентификации и управления сессией пользователя.
/// Определяет контракты для работы с авторизацией и текущим контекстом пользователя.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Получает значение, указывающее, аутентифицирован ли текущий пользователь.
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Получает значение, указывающее, имеет ли текущий пользователь права администратора.
    /// </summary>
    bool IsAdmin { get; }

    /// <summary>
    /// Получает полное имя текущего пользователя.
    /// </summary>
    string? FullName { get; }

    /// <summary>
    /// Получает идентификатор текущего пользователя.
    /// </summary>
    int? IdCurrentUser { get; }

    /// <summary>
    /// Получает идентификатор администратора системы.
    /// </summary>
    int IdAdmin { get; }

    /// <summary>
    /// Выполняет аутентификацию пользователя по учетным данным.
    /// </summary>
    /// <param name="employeeLoginModel">Модель с учетными данными для входа</param>
    /// <returns>true, если аутентификация успешна, иначе false</returns>
    Task<bool> LoginAsync(EmployeeLoginModel employeeLoginModel);

    /// <summary>
    /// Выполняет выход текущего пользователя из системы.
    /// </summary>
    Task LogoutAsync();
}