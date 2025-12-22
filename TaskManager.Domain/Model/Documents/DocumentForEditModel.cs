namespace TaskManager.Domain.Model.Documents;

/// <summary>
/// Модель документа для отображения информации об документе при редактировании
/// </summary>
public class DocumentForEditModel
{
    /// <summary>
    /// Уникальный идентификатор документа в системе.
    /// </summary>
    public int Id { get; init; }

    //Входные данные документа

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
    /// Ответственный отдел. Входные данные документа. Заполняет хозяин записи (делопроизводитель).
    /// Необязательное свойство.
    /// </summary>
    public required string? ResponsibleDepartmentInputDocument { get; set; }

    /// <summary>
    /// Привлечённые отделы. Входные данные документа. Заполняет хозяин записи (делопроизводитель).
    /// Обязательное свойство.
    /// </summary>
    public required string? ResponsibleDepartmentsInputDocument { get; init; }

    /// <summary>
    /// Срок выполнения задачи. Входные данные документа. Заполняет хозяин записи (делопроизводитель).
    /// Обязательное свойство.
    /// </summary>
    public required DateOnly TaskDueDateInputDocument { get; init; }

    /// <summary>
    /// Идентификатор ответственного сотрудника. Входные данные документа. Заполняет исполнитель.
    /// Обязательное свойство.
    /// </summary>
    public required int? IdResponsibleEmployeeInputDocument { get; init; }

    //Выходные данные документа

    /// <summary>
    /// Признак внешнего документа. Выходные данные документа. Заполняет исполнитель.
    /// Обязательное свойство.
    /// </summary>
    public required bool IsExternalDocumentOutputDocument { get; init; }

    /// <summary>
    /// Исходящий номер документа Исх(46 ЦНИИ). Выходные данные документа. Заполняет исполнитель.
    /// Необязательное свойство.
    /// </summary>
    public required string? OutgoingDocumentNumberOutputDocument { get; init; }

    /// <summary>
    /// Дата исходящий документа. Выходные данные документа. Заполняет исполнитель.
    /// Необязательное свойство.
    /// </summary>
    public required DateOnly? OutgoingDocumentDateOutputDocument { get; init; }

    /// <summary>
    /// Получатель. Выходные данные документа. Заполняет исполнитель.
    /// Необязательное свойство.
    /// </summary>
    public required string? RecipientOutputDocument { get; init; }

    /// <summary>
    /// Краткое содержание документа. Выходные данные документа. Заполняет исполнитель.
    /// Необязательное свойство.
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
    /// Идентификатор сотрудника, который последним редактировал документ. Выходные данные документа.
    /// Обязательное свойство.
    /// </summary>
    public required string? FullNameLastEditedEmployee { get; init; }

    /// <summary>
    /// Дата и время последнего редактирования документа.
    /// Обязательное свойство.
    /// </summary>
    public required DateTime? LastEditedDateTime { get; init; }

    /// <summary>
    /// Идентификатор сотрудника, который создал документ.
    /// Обязательное свойство.
    /// </summary>
    public required int CreatedByEmployeeId { get; init; }

    /// <summary>
    /// Идентификатор сотрудника, который удалил запись.
    /// Обязательное свойство.
    /// </summary>
    public required int? RemovedByEmployeeId { get; init; }

    /// <summary>
    /// Дата удаления документа.
    /// Обязательное свойство.
    /// </summary>
    public required DateTime? RemoveDateTime { get; init; }
}