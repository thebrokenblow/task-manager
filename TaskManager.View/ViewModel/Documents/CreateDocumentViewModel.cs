using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManager.Application.Dtos.Documents;

namespace TaskManager.View.ViewModel.Documents;

/// <summary>
/// Модель представления для создания документа.
/// </summary>
public sealed class CreateDocumentViewModel
{
    /// <summary>
    /// Документ для создания.
    /// </summary>
    public required CreatedDocumentDto Document { get; init; }

    /// <summary>
    /// Список ответственных сотрудников для выбора.
    /// </summary>
    public required SelectList ResponsibleEmployees { get; init; }

    /// <summary>
    /// Список ответственных отделов для выбора.
    /// </summary>
    public required SelectList ResponsibleDepartments { get; init; }
}