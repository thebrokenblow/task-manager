namespace TaskManager.Domain.Model.Documents.Edit;

/// <summary>
/// Модель для изменения статуса документа
/// Содержит выходные данные документа для обновления статуса
/// </summary>
public sealed class DocumentForChangeStatusModel
{
    /// <summary>
    /// Исходящий номер документа Исх(46 ЦНИИ). Выходные данные документа. Заполняет исполнитель.
    /// Обязательное свойство.
    /// </summary>
    public required string? OutgoingDocumentNumberOutputDocument { get; init; }

    /// <summary>
    /// Дата исходящий документа. Выходные данные документа. Заполняет исполнитель.
    /// Обязательное свойство.
    /// </summary>
    public required DateOnly? OutgoingDocumentDateOutputDocument { get; init; }

    /// <summary>
    /// Получатель. Выходные данные документа. Заполняет исполнитель.
    /// Обязательное свойство.
    /// </summary>
    public required string? RecipientOutputDocument { get; init; }

    /// <summary>
    /// Краткое содержание документа. Выходные данные документа. Заполняет исполнитель.
    /// Обязательное свойство.
    /// </summary>
    public required string? DocumentSummaryOutputDocument { get; init; }

    /// <summary>
    /// Признак завершения задачи. Заполняет исполнитель.
    /// Обязательное свойство.
    /// </summary>
    public required bool IsCompleted { get; init; }
}