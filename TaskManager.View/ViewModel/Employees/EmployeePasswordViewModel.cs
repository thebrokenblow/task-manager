namespace TaskManager.View.ViewModel.Employees;

/// <summary>
/// Модель представления для изменения пароля сотрудника.
/// </summary>
public sealed class EmployeePasswordViewModel
{
    /// <summary>
    /// Идентификатор сотрудника.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Новый пароль сотрудника.
    /// </summary>
    public required string Password { get; set; }
}