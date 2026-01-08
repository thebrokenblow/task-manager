namespace TaskManager.Application.Dtos.Documents;

/// <summary>
/// DTO для редактирования документа в системе документооборота.
/// Содержит данные документа для обновления записи.
/// </summary>
public sealed class EditedDocumentDto
{
    /// <summary>
    /// Уникальный идентификатор документа в системе.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Исходный номер документа. Входные данные документа.
    /// Необязательное свойство.
    /// </summary>
    public string? OutgoingDocumentNumberInputDocument { get; set; }

    /// <summary>
    /// Дата исходного документа. Входные данные документа.
    /// Необязательное свойство.
    /// </summary>
    public DateOnly? SourceDocumentDateInputDocument { get; set; }

    /// <summary>
    /// Заказчик. Входные данные документа.
    /// Необязательное свойство.
    /// </summary>
    public string? CustomerInputDocument { get; set; }

    /// <summary>
    /// Краткое содержание документа. Входные данные документа.
    /// Необязательное свойство.
    /// </summary>
    public string? DocumentSummaryInputDocument { get; set; }

    /// <summary>
    /// Признак внешнего документа. Входные данные документа.
    /// Обязательное свойство.
    /// </summary>
    public required bool IsExternalDocumentInputDocument { get; set; }

    /// <summary>
    /// Входящий номер документа ВХ(46 ЦНИИ). Входные данные документа.
    /// Обязательное свойство.
    /// </summary>
    public required string IncomingDocumentNumberInputDocument { get; set; }

    /// <summary>
    /// Дата входящего документа. Входные данные документа.
    /// Обязательное свойство.
    /// </summary>
    public required DateOnly IncomingDocumentDateInputDocument { get; set; }

    /// <summary>
    /// Ответственный отдел. Входные данные документа.
    /// Необязательное свойство.
    /// </summary>
    public string? ResponsibleDepartmentInputDocument { get; set; }

    /// <summary>
    /// Ответственные отделы. Входные данные документа.
    /// Необязательное свойство.
    /// </summary>
    public string? ResponsibleDepartmentsInputDocument { get; set; }

    /// <summary>
    /// Срок выполнения задачи. Входные данные документа.
    /// Обязательное свойство.
    /// </summary>
    public required DateOnly TaskDueDateInputDocument { get; set; }

    /// <summary>
    /// Идентификатор ответственного сотрудника. Входные данные документа.
    /// Необязательное свойство.
    /// </summary>
    public int? IdResponsibleEmployeeInputDocument { get; set; }

    /// <summary>
    /// Признак внешнего документа. Выходные данные документа.
    /// Обязательное свойство.
    /// </summary>
    public required bool IsExternalDocumentOutputDocument { get; set; }

    /// <summary>
    /// Исходящий номер документа Исх(46 ЦНИИ). Выходные данные документа.
    /// Необязательное свойство.
    /// </summary>
    public string? OutgoingDocumentNumberOutputDocument { get; set; }

    /// <summary>
    /// Дата исходящего документа. Выходные данные документа.
    /// Необязательное свойство.
    /// </summary>
    public DateOnly? OutgoingDocumentDateOutputDocument { get; set; }

    /// <summary>
    /// Получатель. Выходные данные документа.
    /// Необязательное свойство.
    /// </summary>
    public string? RecipientOutputDocument { get; set; }

    /// <summary>
    /// Краткое содержание документа. Выходные данные документа.
    /// Необязательное свойство.
    /// </summary>
    public string? DocumentSummaryOutputDocument { get; set; }

    /// <summary>
    /// Признак нахождения задачи на контроле.
    /// Обязательное свойство.
    /// </summary>
    public required bool IsUnderControl { get; set; }

    /// <summary>
    /// Признак завершения задачи.
    /// Обязательное свойство.
    /// </summary>
    public required bool IsCompleted { get; set; }

    /// <summary>
    /// Тематика документа. Выходные данные документа.
    /// Необязательное свойство.
    /// </summary>
    public required string? SubjectOutputDocument { get; set; }
}