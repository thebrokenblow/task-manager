using TaskManager.Domain.Model.Departments;

namespace TaskManager.Application.Services.Interfaces;

/// <summary>
/// Сервис для работы с подразделениями.
/// Предоставляет бизнес-логику для операций с подразделениями.
/// </summary>
public interface IDepartmentService
{
    /// <summary>
    /// Получает все подразделения из системы.
    /// </summary>
    /// <returns>
    /// Задача, результат которой содержит перечисление моделей <see cref="DepartmentSelectModel"/>.
    /// </returns>
    Task<IEnumerable<DepartmentSelectModel>> GetDepartmentsAsync();
}