namespace TaskManager.Application.Dtos.Documents;

/// <summary>
/// DTO для фильтрации документов. Содержит критерии поиска.
/// </summary>
public sealed class DocumentFilterDto
{
    /// <summary>
    /// Поисковый термин для фильтрации по тексту документов.
    /// </summary>
    public required string? SearchTerm { get; init; }

    /// <summary>
    /// Фильтр по документам текущего пользователя.
    /// </summary>
    public required bool IsShowMyDocuments { get; init; }

    /// <summary>
    /// Начальная дата диапазона для даты исходящего документа.
    /// </summary>
    public required DateOnly? StartOutgoingDocumentDateOutputDocument { get; init; }

    /// <summary>
    /// Конечная дата диапазона для даты исходящего документа.
    /// </summary>
    public required DateOnly? EndOutgoingDocumentDateOutputDocument { get; init; }
}