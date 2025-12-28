namespace TaskManager.Application.Dtos.Documents;

public sealed class EditedDocumentDto
{
    public int Id { get; set; }
    public string? OutgoingDocumentNumberInputDocument { get; set; }
    public DateOnly? SourceDocumentDateInputDocument { get; set; }
    public string? CustomerInputDocument { get; set; }
    public string? DocumentSummaryInputDocument { get; set; }
    public required bool IsExternalDocumentInputDocument { get; set; }
    public required string IncomingDocumentNumberInputDocument { get; set; }
    public required DateOnly IncomingDocumentDateInputDocument { get; set; }
    public string? ResponsibleDepartmentInputDocument { get; set; }
    public string? ResponsibleDepartmentsInputDocument { get; set; }
    public required DateOnly TaskDueDateInputDocument { get; set; }
    public int? IdResponsibleEmployeeInputDocument { get; set; }
    public required bool IsExternalDocumentOutputDocument { get; set; }
    public string? OutgoingDocumentNumberOutputDocument { get; set; }
    public DateOnly? OutgoingDocumentDateOutputDocument { get; set; }
    public string? RecipientOutputDocument { get; set; }
    public string? DocumentSummaryOutputDocument { get; set; }
    public required bool IsUnderControl { get; set; }
    public required bool IsCompleted { get; set; }
    public required string? SubjectOutputDocument { get; set; }
}