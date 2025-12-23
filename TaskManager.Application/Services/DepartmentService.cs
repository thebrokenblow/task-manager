using TaskManager.Application.Services.Interfaces;
using TaskManager.Domain.Model.Departments.Query;
using TaskManager.Domain.Queries;

namespace TaskManager.Application.Services;

/// <summary>
/// Сервис для работы с подразделениями.
/// Предоставляет бизнес-логику для операций с подразделениями.
/// </summary>
public sealed class DepartmentService(IDepartmentQuery departmentQuery) : IDepartmentService
{
    private readonly IDepartmentQuery _departmentQuery =
        departmentQuery ?? throw new ArgumentNullException(nameof(departmentQuery));

    /// <summary>
    /// Получает все подразделения из системы.
    /// </summary>
    /// <returns>
    /// Задача, результат которой содержит перечисление моделей <see cref="DepartmentSelectModel"/>.
    /// </returns>
    public async Task<IEnumerable<DepartmentSelectModel>> GetDepartmentsAsync()
    {
        var departments = await _departmentQuery.GetDepartmentsAsync();

        return departments;
    }
}