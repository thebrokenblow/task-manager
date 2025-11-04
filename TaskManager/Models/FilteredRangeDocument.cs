using TaskManager.Controllers;

namespace TaskManager.Models;

public class FilteredRangeDocument
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
    /// Идентификатор ответственного сотрудника из исходных данных
    /// Обязательное поле
    /// </summary>
    public required int SourceResponsibleEmployeeId { get; set; }

    /// <summary>
    /// Ответственный сотрудник из исходных данных
    /// </summary>
    public required Employee SourceResponsibleEmployee { get; set; }

    // Выходные данные документа

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
    /// Логин автора задания
    /// Обязательное поле
    /// </summary>
    public required string LoginAuthor { get; set; }

    /// <summary>
    /// Признак завершения задачи
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Дата удаления документа
    /// </summary>
    public DateTime? DateRemove { get; set; }

    public bool IsNotDeletedDocument => 
        LoginAuthor != AccountsController.AdminLogin && 
        DateRemove == null;
}