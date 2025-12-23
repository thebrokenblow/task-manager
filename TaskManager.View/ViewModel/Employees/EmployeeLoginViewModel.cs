namespace TaskManager.View.ViewModel.Employees;

/// <summary>
/// Модель представления для входа сотрудника.
/// </summary>
public sealed class EmployeeLoginViewModel
{
    /// <summary>
    /// Логин сотрудника.
    /// </summary>
    public required string Login { get; set; }

    /// <summary>
    /// Пароль сотрудника.
    /// </summary>
    public required string Password { get; set; }

    /// <summary>
    /// URL для возврата после входа.
    /// </summary>
    public string? ReturnUrl { get; set; }
}