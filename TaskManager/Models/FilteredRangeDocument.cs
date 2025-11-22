using TaskManager.Controllers;

<<<<<<<< HEAD:TaskManager/ViewModel/DocumentOverviewModel.cs
namespace TaskManager.Domain.Models;

public class DocumentOverviewModel
========
namespace TaskManager.Models;

public class FilteredRangeDocument
>>>>>>>> 82971907aa47c5a90b270b82a3d0e787f6f81a10:TaskManager/Models/FilteredRangeDocument.cs
{
    /// <summary>
    /// Уникальный идентификатор документа
    /// </summary>
    public required int Id { get; set; }

    // Исходные данные документа

    /// <summary>
    /// Номер исходящего документа (outgoing) из исходных данных
    /// Обязательное поле
    /// </summary>
    public required string SourceOutgoingDocumentNumber { get; set; }

    /// <summary>
    /// Текст задачи из исходных данных
    /// Обязательное поле
    /// </summary>
    public required string SourceTaskText { get; set; }

    /// <summary>
    /// Номер выходящего документа (output) из исходных данных
    /// Обязательное поле
    /// </summary>
    public required string SourceOutputDocumentNumber { get; set; }

    /// <summary>
    /// Срок выполнения из исходных данных
    /// Обязательное поле
    /// </summary>
    public required DateOnly SourceDueDate { get; set; }


    /// <summary>
    /// Номер исходящего документа из выходных данных
    /// </summary>
    public required string? OutputOutgoingNumber { get; set; }

    /// <summary>
    /// Дата исходящего документа из выходных данных
    /// </summary>
    public required DateOnly? OutputOutgoingDate { get; set; }

    /// <summary>
    /// Признак нахождения задачи на контроле
    /// </summary>
    public bool IsUnderControl { get; set; }

    /// <summary>
    /// Идентификатор пользователя, который создал запись
    /// Обязательное поле
    /// </summary>
    public required int IdAuthorCreateDocument { get; set; }

    /// <summary>
    /// Навигационно свойство пользователя, который создал запись
    /// Обязательное поле
    /// </summary>
    public required Employee AuthorCreateDocument { get; set; }

    /// <summary>
    /// Признак завершения задачи
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Дата удаления документа
    /// </summary>
    public DateTime? DateRemove { get; set; }

    /// <summary>
    /// Идентификатор пользователя, который удалил запись
    /// Обязательное поле
    /// </summary>
    public int? IdAuthorRemoveDocument { get; set; }
}