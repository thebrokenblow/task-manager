namespace TaskManager.Domain.Model.Documents;

public class DocumentForOverviewModel
{
    /// <summary>
    /// Уникальный идентификатор документа в системе.
    /// </summary>
    public required int Id { get; init; }

    /// <summary>
    /// Исходный номер документа. Входные данные документа. Заполняет хозяин записи (делопроизводитель).
    /// Обязательное свойство.
    /// </summary>
    public required string? OutgoingDocumentNumberInputDocument { get; init; }

    /// <summary>
    /// Краткое содержание документа. Входные данные документа. Заполняет хозяин записи (делопроизводитель).
    /// Обязательное свойство.
    /// </summary>
    public required string? DocumentSummaryInputDocument { get; init; }

    /// <summary>
    /// Входящий номер документа ВХ(46 ЦНИИ). Входные данные документа. Заполняет хозяин записи (делопроизводитель).
    /// Обязательное свойство.
    /// </summary>
    public required string IncomingDocumentNumberInputDocument { get; init; }

    /// <summary>
    /// Заказчик. Входные данные документа. Заполняет хозяин записи (делопроизводитель).
    /// Обязательное свойство.
    /// </summary>
    public required string? CustomerInputDocument { get; init; }

    /// <summary>
    /// Срок выполнения задачи. Входные данные документа. Заполняет хозяин записи (делопроизводитель).
    /// Обязательное свойство.
    /// </summary>
    public required DateOnly TaskDueDateInputDocument { get; init; }

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
    /// Признак нахождения задачи на контроле. Выходные данные документа. Заполняет хозяин записи (делопроизводитель).
    /// Обязательное свойство.
    /// </summary>
    public required bool IsUnderControl { get; init; }

    /// <summary>
    /// Полное имя ответственного сотрудника. Входные данные документа. Заполняется автоматически на основе IdResponsibleEmployeeInputDocument.
    /// Обязательное свойство (может быть null).
    /// </summary>
    public required string? FullNameResponsibleEmployee { get; init; }

    /// <summary>
    /// Идентификатор сотрудника, который создал документ.
    /// Обязательное свойство.
    /// </summary>
    public required int CreatedByEmployeeId { get; init; }

    /// <summary>
    /// Признак завершения задачи. Заполняет исполнитель.
    /// Обязательное свойство.
    /// </summary>
    public required bool IsCompleted { get; init; }

    /// <summary>
    /// Идентификатор сотрудника, который удалил запись.
    /// Обязательное свойство (может быть null).
    /// </summary>
    public required int? RemovedByEmployeeId { get; init; }

    /// <summary>
    /// Дата удаления документа.
    /// Обязательное свойство (может быть null).
    /// </summary>
    public required DateTime? RemoveDateTime { get; init; }
}