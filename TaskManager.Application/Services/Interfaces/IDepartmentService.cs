using TaskManager.Domain.Model.Departments;

namespace TaskManager.Application.Services.Interfaces;

/// <summary>
/// Сервис для работы с подразделениями.
/// </summary>
public interface IDepartmentService
{
    /// <summary>
    /// Получает все подразделения.
    /// </summary>
    Task<IEnumerable<DepartmentSelectModel>> GetAllAsync();
}