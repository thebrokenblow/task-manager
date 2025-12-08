namespace TaskManager.Domain.Model.Employees;

/// <summary>
/// Модель для аутентификации сотрудника в системе
/// Содержит учетные данные для входа в систему
/// </summary>
public class EmployeeLoginModel
{
    /// <summary>
    /// Логин сотрудника для входа в систему
    /// Обязательное свойство
    /// </summary>
    public required string Login { get; set; }

    /// <summary>
    /// Пароль сотрудника для входа в систему
    /// Обязательное свойство
    /// </summary>
    public required string Password { get; set; }
}