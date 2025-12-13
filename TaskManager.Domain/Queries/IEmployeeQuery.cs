using TaskManager.Domain.Entities;
using TaskManager.Domain.Model.Employees;

namespace TaskManager.Domain.Queries;

/// <summary>
/// Интерфейс для выполнения запросов к данным сотрудников.
/// Определяет контракты для получения информации о сотрудниках в различных представлениях и состояниях.
/// Интерфейс служит абстракцией для взаимодействия с хранилищем данных (базой данных).
/// </summary>
public interface IEmployeeQuery
{
    /// <summary>
    /// Получает список обычных сотрудников.
    /// </summary>
    /// <returns>Список сотрудников</returns>
    Task<List<Employee>> GetRegularEmployeesAsync();

    /// <summary>
    /// Получает список сотрудников, которые могут быть назначены ответственными.
    /// </summary>
    /// <returns>Список моделей сотрудников для выбора</returns>
    Task<List<EmployeeSelectModel>> GetResponsibleEmployeesAsync(string department);
}