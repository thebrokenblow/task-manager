namespace TaskManager.Domain.Model.Documents;

/// <summary>
/// Модель для выгрузки в Csv формат
/// </summary>
public class DocumentForCsvExportModel
{
    /// <summary>
    /// Исходный номер документа. Входные данные документа. Заполняет хозяин записи (делопроизводитель).
    /// Обязательное свойство.
    /// </summary>
    public required string? OutgoingDocumentNumberInputDocument { get; init; }

    /// <summary>
    /// Дата исходного документа. Входные данные документа. Заполняет хозяин записи (делопроизводитель).
    /// Обязательное свойство.
    /// </summary>
    public required DateOnly? SourceDocumentDateInputDocument { get; init; }

    /// <summary>
    /// Заказчик. Входные данные документа. Заполняет хозяин записи (делопроизводитель).
    /// Обязательное свойство.
    /// </summary>
    public required string? CustomerInputDocument { get; init; }

    /// <summary>
    /// Краткое содержание документа. Входные данные документа. Заполняет хозяин записи (делопроизводитель).
    /// Обязательное свойство.
    /// </summary>
    public required string? DocumentSummaryInputDocument { get; init; }

    /// <summary>
    /// Признак внешнего документа. Входные данные документа. Заполняет хозяин записи (делопроизводитель).
    /// Обязательное свойство.
    /// </summary>
    public required bool IsExternalDocumentInputDocument { get; init; }

    /// <summary>
    /// Входящий номер документа ВХ(46 ЦНИИ). Входные данные документа. Заполняет хозяин записи (делопроизводитель).
    /// Обязательное свойство.
    /// </summary>
    public required string IncomingDocumentNumberInputDocument { get; init; }

    /// <summary>
    /// Дата входящего документа. Входные данные документа. Заполняет хозяин записи (делопроизводитель).
    /// Обязательное свойство.
    /// </summary>
    public required DateOnly IncomingDocumentDateInputDocument { get; init; }

    /// <summary>
    /// Ответственные отделы. Входные данные документа. Заполняет хозяин записи (делопроизводитель).
    /// Обязательное свойство.
    /// </summary>
    public required string? ResponsibleDepartmentsInputDocument { get; init; }

    /// <summary>
    /// Срок выполнения задачи. Входные данные документа. Заполняет хозяин записи (делопроизводитель).
    /// Обязательное свойство.
    /// </summary>
    public required DateOnly TaskDueDateInputDocument { get; init; }

    /// <summary>
    /// ФИО ответственного сотрудника. Входные данные документа. Заполняет исполнитель.
    /// Навигационное свойство.
    /// Обязательное свойство.
    /// </summary>
    public required string? FullNameResponsibleEmployeeInputDocument { get; init; }

    //Выходные данные документа

    /// <summary>
    /// Признак внешнего документа. Выходные данные документа. Заполняет исполнитель.
    /// Обязательное свойство.
    /// </summary>
    public required bool IsExternalDocumentOutputDocument { get; init; }

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
    /// Признак нахождения задачи на контроле. Выходные данные документа. Заполняет хозяин записи (делопроизводитель).
    /// Обязательное свойство.
    /// </summary>
    public required bool IsUnderControl { get; init; }

    /// <summary>
    /// Признак завершения задачи. Заполняет исполнитель.
    /// Обязательное свойство.
    /// </summary>
    public required bool IsCompleted { get; init; }

    /// <summary>
    /// ФИО сотрудника, который создал документ.
    /// Обязательное свойство.
    /// </summary>
    public required string FullNameCreatedEmployee { get; init; }
}