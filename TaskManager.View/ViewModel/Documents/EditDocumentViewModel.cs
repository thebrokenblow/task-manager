using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManager.Domain.Model.Documents;

namespace TaskManager.View.ViewModel.Documents;

/// <summary>
/// Модель представления для редактирования документа.
/// </summary>
public class EditDocumentViewModel
{
    /// <summary>
    /// Документ для редактирования.
    /// </summary>
    public required DocumentForOverviewEditModel Document { get; init; }

    /// <summary>
    /// Список ответственных сотрудников для выбора.
    /// </summary>
    public required SelectList ResponsibleEmployees { get; init; }

    /// <summary>
    /// Сообщение об ошибке, если есть.
    /// </summary>
    public string? ErrorMessage { get; init; }

    /// <summary>
    /// Список ответственных отделов для выбора.
    /// </summary>
    public required SelectList ResponsibleDepartments { get; init; }
}