using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManager.Application.Common;
using TaskManager.Domain.Model.Documents;

namespace TaskManager.View.ViewModel.Employees;

/// <summary>
/// Модель представления для отображения списка документов.
/// </summary>
public class IndexDocumentViewModel
{
    /// <summary>
    /// Строка поиска.
    /// </summary>
    public required string InputString { get; init; }

    /// <summary>
    /// Флаг отображения только моих задач.
    /// </summary>
    public required bool IsShowMyDocuments { get; init; }

    /// <summary>
    /// Список доступных размеров страницы.
    /// </summary>
    public required SelectList CountsDocumentsOnPage { get; init; }

    /// <summary>
    /// Начальная дата для фильтрации исходящих документов.
    /// </summary>
    public required DateOnly? StartOutgoingDocumentDateOutputDocument { get; init; }

    /// <summary>
    /// Конечная дата для фильтрации исходящих документов.
    /// </summary>
    public required DateOnly? EndOutgoingDocumentDateOutputDocument { get; init; }

    /// <summary>
    /// Постраничный список документов.
    /// </summary>
    public required PagedResult<DocumentForOverviewModel> PagedDocuments { get; init; }
}