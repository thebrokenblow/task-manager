namespace TaskManager.Models;

/// <summary>
/// Представляет документ в системе документооборота
/// Содержит исходные данные документа и выходные данные обработки
/// </summary>
public class Document
{
    /// <summary>
    /// Уникальный идентификатор документа
    /// </summary>
    public int Id { get; set; }

    // Исходные данные документа

    /// <summary>
    /// Номер исходящего документа (outgoing) из исходных данных
    /// Обязательное поле
    /// </summary>
    public required string SourceOutgoingDocumentNumber { get; set; }

    /// <summary>
    /// Дата исходящего документа из исходных данных
    /// Обязательное поле
    /// </summary>
    public required DateOnly SourceOutgoingDocumentDate { get; set; }

    /// <summary>
    /// Заказчик из исходных данных
    /// Обязательное поле
    /// </summary>
    public required string SourceCustomer { get; set; }

    /// <summary>
    /// Текст задачи из исходных данных
    /// Обязательное поле
    /// </summary>
    public required string SourceTaskText { get; set; }

    /// <summary>
    /// Признак внешнего документа из исходных данных
    /// </summary>
    public required bool SourceIsExternal { get; set; }

    /// <summary>
    /// Номер выходящего документа (output) из исходных данных
    /// Обязательное поле
    /// </summary>
    public required string SourceOutputDocumentNumber { get; set; }

    /// <summary>
    /// Дата входящего документа из исходных данных
    /// Обязательное поле
    /// </summary>
    public required DateOnly SourceOutputDocumentDate { get; set; }

    /// <summary>
    /// Срок выполнения из исходных данных
    /// Обязательное поле
    /// </summary>
    public required DateOnly SourceDueDate { get; set; }

    /// <summary>
    /// Идентификатор ответственного сотрудника
    /// Обязательное поле
    /// </summary>
    public required int IdSourceResponsibleEmployee { get; set; }

    /// <summary>
    /// Ответственный сотрудник
    /// </summary>
    public Employee? SourceResponsibleEmployee { get; set; }

    // Выходные данные документа

    /// <summary>
    /// Номер исходящего документа из выходных данных
    /// </summary>
    public string? OutputOutgoingNumber { get; set; }

    /// <summary>
    /// Дата исходящего документа из выходных данных
    /// </summary>
    public DateOnly? OutputOutgoingDate { get; set; }

    /// <summary>
    /// Получатель документа из выходных данных (кому отправлен)
    /// </summary>
    public string? OutputSentTo { get; set; }

    /// <summary>
    /// Информация о передаче в рабочем порядке из выходных данных
    /// </summary>
    public string? OutputTransferredInWorkOrder { get; set; }

    /// <summary>
    /// Признак сдачи ответа из выходных данных
    /// </summary>
    public string? OutputResponseSubmissionMark { get; set; }

    /// <summary>
    /// Признак нахождения задачи на контроле
    /// </summary>
    public bool IsUnderControl { get; set; }

    /// <summary>
    /// Признак завершения задачи
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Идентификатор пользователя, который создал запись
    /// Обязательное поле
    /// </summary>
    public required int IdAuthorCreateDocument { get; set; }

    /// <summary>
    /// Навигационно свойство пользователя, который создал запись 
    /// </summary>
    public User? AuthorCreateDocument { get; set; }

    /// <summary>
    /// Идентификатор пользователя, который удалил запись
    /// Обязательное поле
    /// </summary>
    public required int? IdAuthorRemoveDocument { get; set; }

    /// <summary>
    /// Навигационно свойство пользователя, который удалил запись
    /// </summary>
    public User? AuthorRemoveDocument { get; set; }

    /// <summary>
    /// Дата удаления документа
    /// </summary>
    public DateTime? DateRemove { get; set; }
}