namespace TaskManager.Application.Dtos.Documents;

/// <summary>
/// DTO для создания документа в системе документооборота.
/// Содержит данные документа для создания записи.
/// </summary>
public sealed class CreatedDocumentDto
{
    /// <summary>
    /// Исходный номер документа. Входные данные документа. Заполняет хозяин записи (делопроизводитель).
    /// Необязательное свойство.
    /// </summary>
    public required string? OutgoingDocumentNumberInputDocument { get; set; }

    /// <summary>
    /// Дата исходного документа. Входные данные документа. Заполняет хозяин записи (делопроизводитель).
    /// Необязательное свойство.
    /// </summary>
    public required DateOnly? SourceDocumentDateInputDocument { get; set; }

    /// <summary>
    /// Заказчик. Входные данные документа. Заполняет хозяин записи (делопроизводитель).
    /// Необязательное свойство.
    /// </summary>
    public required string? CustomerInputDocument { get; set; }

    /// <summary>
    /// Краткое содержание документа. Входные данные документа. Заполняет хозяин записи (делопроизводитель).
    /// Необязательное свойство.
    /// </summary>
    public required string? DocumentSummaryInputDocument { get; set; }

    /// <summary>
    /// Признак внешнего документа. Входные данные документа. Заполняет хозяин записи (делопроизводитель).
    /// Обязательное свойство.
    /// </summary>
    public required bool IsExternalDocumentInputDocument { get; set; }

    /// <summary>
    /// Входящий номер документа ВХ(46 ЦНИИ). Входные данные документа. Заполняет хозяин записи (делопроизводитель).
    /// Обязательное свойство.
    /// </summary>
    public required string IncomingDocumentNumberInputDocument { get; set; }

    /// <summary>
    /// Дата входящего документа. Входные данные документа. Заполняет хозяин записи (делопроизводитель).
    /// Обязательное свойство.
    /// </summary>
    public required DateOnly IncomingDocumentDateInputDocument { get; set; }

    /// <summary>
    /// Ответственный отдел. Входные данные документа. Заполняет хозяин записи (делопроизводитель).
    /// Необязательное свойство.
    /// </summary>
    public required string? ResponsibleDepartmentInputDocument { get; set; }

    /// <summary>
    /// Ответственные отделы. Входные данные документа. Заполняет хозяин записи (делопроизводитель).
    /// Необязательное свойство.
    /// </summary>
    public required string? ResponsibleDepartmentsInputDocument { get; set; }

    /// <summary>
    /// Срок выполнения задачи. Входные данные документа. Заполняет хозяин записи (делопроизводитель).
    /// Обязательное свойство.
    /// </summary>
    public required DateOnly TaskDueDateInputDocument { get; set; }

    /// <summary>
    /// Идентификатор ответственного сотрудника. Входные данные документа. Заполняет исполнитель.
    /// Необязательное свойство.
    /// </summary>
    public required int? IdResponsibleEmployeeInputDocument { get; set; }

    /// <summary>
    /// Признак внешнего документа. Выходные данные документа. Заполняет исполнитель.
    /// Обязательное свойство.
    /// </summary>
    public required bool IsExternalDocumentOutputDocument { get; set; }

    /// <summary>
    /// Исходящий номер документа Исх(46 ЦНИИ). Выходные данные документа. Заполняет исполнитель.
    /// Необязательное свойство.
    /// </summary>
    public required string? OutgoingDocumentNumberOutputDocument { get; set; }

    /// <summary>
    /// Дата исходящего документа. Выходные данные документа. Заполняет исполнитель.
    /// Необязательное свойство.
    /// </summary>
    public required DateOnly? OutgoingDocumentDateOutputDocument { get; set; }

    /// <summary>
    /// Получатель. Выходные данные документа. Заполняет исполнитель.
    /// Необязательное свойство.
    /// </summary>
    public required string? RecipientOutputDocument { get; set; }

    /// <summary>
    /// Краткое содержание документа. Выходные данные документа. Заполняет исполнитель.
    /// Необязательное свойство.
    /// </summary>
    public required string? DocumentSummaryOutputDocument { get; set; }

    /// <summary>
    /// Признак нахождения задачи на контроле. Выходные данные документа. Заполняет хозяин записи (делопроизводитель).
    /// Обязательное свойство.
    /// </summary>
    public required bool IsUnderControl { get; set; }

    /// <summary>
    /// Тематика документа. Выходные данные документа. Заполняет исполнитель.
    /// Необязательное свойство.
    /// </summary>
    public required string? SubjectOutputDocument { get; set; }
}
