using TaskManager.Domain.Model.Departments;

namespace TaskManager.Application.Services.Interfaces;

/// <summary>
/// Сервис для работы с подразделениями.
/// Предоставляет бизнес-логику для операций с подразделениями.
/// </summary>
public interface IDepartmentService
{
    /// <summary>
    /// Сервис для работы с подразделениями.
    /// Предоставляет бизнес-логику для операций с подразделениями.
    /// </summary>
    Task<IEnumerable<DepartmentSelectModel>> GetDepartmentsAsync();
}