using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManager.Domain.Entities;

namespace TaskManager.View.ViewModel.Documents;

/// <summary>
/// Модель представления для создания документа.
/// </summary>
public class CreateDocumentViewModel
{
    /// <summary>
    /// Документ для создания.
    /// </summary>
    public required Document Document { get; init; }

    /// <summary>
    /// Список ответственных сотрудников для выбора.
    /// </summary>
    public required SelectList ResponsibleEmployees { get; init; }

    /// <summary>
    /// Список ответственных отделов для выбора.
    /// </summary>
    public required SelectList ResponsibleDepartments { get; init; }
}