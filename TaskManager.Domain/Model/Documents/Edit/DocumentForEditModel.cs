namespace TaskManager.Domain.Model.Documents.Edit;

public sealed class DocumentForEditModel
{
    public required int Id { get; init; }
    public required string? OutgoingDocumentNumberInputDocument { get; init; }
    public required DateOnly? SourceDocumentDateInputDocument { get; init; }
    public required string? CustomerInputDocument { get; init; }
    public required string? DocumentSummaryInputDocument { get; init; }
    public required bool IsExternalDocumentInputDocument { get; init; }
    public required string IncomingDocumentNumberInputDocument { get; init; }
    public required DateOnly IncomingDocumentDateInputDocument { get; init; }
    public required string? ResponsibleDepartmentInputDocument { get; init; }
    public required string? ResponsibleDepartmentsInputDocument { get; init; }
    public required DateOnly TaskDueDateInputDocument { get; init; }
    public required int? IdResponsibleEmployeeInputDocument { get; init; }
    public required bool IsExternalDocumentOutputDocument { get; init; }
    public required string? OutgoingDocumentNumberOutputDocument { get; init; }
    public required DateOnly? OutgoingDocumentDateOutputDocument { get; init; }
    public required string? RecipientOutputDocument { get; init; }
    public required string? DocumentSummaryOutputDocument { get; init; }
    public required bool IsUnderControl { get; init; }
    public required bool IsCompleted { get; init; }
    public required DateTime LastEditedDateTime { get; init; }
    public required int LastEditedByEmployeeId { get; init; }

    /// <summary>
    /// Тематика документа.
    /// Необязательное свойство.
    /// </summary>
    public string? SubjectOutputDocument { get; init; }
}