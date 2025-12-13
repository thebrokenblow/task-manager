namespace TaskManager.Application.Dtos.Documents;

/// <summary>
/// DTO для фильтрации документов. Содержит критерии поиска.
/// </summary>
public class DocumentFilterDto
{
    /// <summary>
    /// Поисковый термин для фильтрации по тексту документов.
    /// </summary>
    public required string? SearchTerm { get; init; }

    /// <summary>
    /// Фильтр по задачам текущего пользователя.
    /// </summary>
    public required bool IsShowMyTasks { get; init; }

    /// <summary>
    /// Начальная дата диапазона для даты исходящего документа.
    /// </summary>
    public required DateOnly? StartOutgoingDocumentDateOutputDocument { get; init; }

    /// <summary>
    /// Конечная дата диапазона для даты исходящего документа.
    /// </summary>
    public required DateOnly? EndOutgoingDocumentDateOutputDocument { get; init; }
}