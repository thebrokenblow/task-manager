namespace TaskManager.Domain.Model.Documents.Edit;

/// <summary>
/// Модель для редактирования документа в системе документооборота.
/// Содержит все поля документа, которые могут быть отредактированы, с историей изменений.
/// </summary>
public sealed class DocumentForEditModel
{
    /// <summary>
    /// Уникальный идентификатор документа в системе.
    /// Обязательное свойство.
    /// </summary>
    public required int Id { get; init; }

    /// <summary>
    /// Исходный номер документа. Входные данные документа.
    /// Необязательное свойство.
    /// </summary>
    public required string? OutgoingDocumentNumberInputDocument { get; init; }

    /// <summary>
    /// Дата исходного документа. Входные данные документа.
    /// Необязательное свойство.
    /// </summary>
    public required DateOnly? SourceDocumentDateInputDocument { get; init; }

    /// <summary>
    /// Заказчик. Входные данные документа.
    /// Необязательное свойство.
    /// </summary>
    public required string? CustomerInputDocument { get; init; }

    /// <summary>
    /// Краткое содержание документа. Входные данные документа.
    /// Необязательное свойство.
    /// </summary>
    public required string? DocumentSummaryInputDocument { get; init; }

    /// <summary>
    /// Признак внешнего документа. Входные данные документа.
    /// Обязательное свойство.
    /// </summary>
    public required bool IsExternalDocumentInputDocument { get; init; }

    /// <summary>
    /// Входящий номер документа ВХ(46 ЦНИИ). Входные данные документа.
    /// Обязательное свойство.
    /// </summary>
    public required string IncomingDocumentNumberInputDocument { get; init; }

    /// <summary>
    /// Дата входящего документа. Входные данные документа.
    /// Обязательное свойство.
    /// </summary>
    public required DateOnly IncomingDocumentDateInputDocument { get; init; }

    /// <summary>
    /// Ответственный отдел. Входные данные документа.
    /// Необязательное свойство.
    /// </summary>
    public required string? ResponsibleDepartmentInputDocument { get; init; }

    /// <summary>
    /// Ответственные отделы. Входные данные документа.
    /// Необязательное свойство.
    /// </summary>
    public required string? ResponsibleDepartmentsInputDocument { get; init; }

    /// <summary>
    /// Срок выполнения задачи. Входные данные документа.
    /// Обязательное свойство.
    /// </summary>
    public required DateOnly TaskDueDateInputDocument { get; init; }

    /// <summary>
    /// Идентификатор ответственного сотрудника. Входные данные документа.
    /// Необязательное свойство.
    /// </summary>
    public required int? IdResponsibleEmployeeInputDocument { get; init; }

    /// <summary>
    /// Признак внешнего документа. Выходные данные документа.
    /// Обязательное свойство.
    /// </summary>
    public required bool IsExternalDocumentOutputDocument { get; init; }

    /// <summary>
    /// Исходящий номер документа Исх(46 ЦНИИ). Выходные данные документа.
    /// Необязательное свойство.
    /// </summary>
    public required string? OutgoingDocumentNumberOutputDocument { get; init; }

    /// <summary>
    /// Дата исходящего документа. Выходные данные документа.
    /// Необязательное свойство.
    /// </summary>
    public required DateOnly? OutgoingDocumentDateOutputDocument { get; init; }

    /// <summary>
    /// Получатель. Выходные данные документа.
    /// Необязательное свойство.
    /// </summary>
    public required string? RecipientOutputDocument { get; init; }

    /// <summary>
    /// Краткое содержание документа. Выходные данные документа.
    /// Необязательное свойство.
    /// </summary>
    public required string? DocumentSummaryOutputDocument { get; init; }

    /// <summary>
    /// Признак нахождения задачи на контроле.
    /// Обязательное свойство.
    /// </summary>
    public required bool IsUnderControl { get; init; }

    /// <summary>
    /// Признак завершения задачи.
    /// Обязательное свойство.
    /// </summary>
    public required bool IsCompleted { get; init; }

    /// <summary>
    /// Дата и время последнего редактирования документа.
    /// Обязательное свойство.
    /// </summary>
    public required DateTime LastEditedDateTime { get; init; }

    /// <summary>
    /// Идентификатор сотрудника, который последним редактировал документ.
    /// Обязательное свойство.
    /// </summary>
    public required int LastEditedByEmployeeId { get; init; }

    /// <summary>
    /// Тематика документа.
    /// Необязательное свойство.
    /// </summary>
    public string? SubjectOutputDocument { get; init; }
}