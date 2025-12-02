namespace TaskManager.Domain.Model.Documents;

public class DocumentForDeleteModel
{
    public int Id { get; set; }

    // Исходные данные документа
    public string? OutgoingDocumentNumberInputDocument { get; set; }
    public DateOnly? SourceDocumentDateInputDocument { get; set; }
    public string? CustomerInputDocument { get; set; }
    public string? DocumentSummaryInputDocument { get; set; }
    public bool IsExternalDocumentInputDocument { get; set; }
    public string? IncomingDocumentNumberInputDocument { get; set; }
    public DateOnly IncomingDocumentDateInputDocument { get; set; }
    public string? ResponsibleDepartmentsInputDocument { get; set; }
    public DateOnly TaskDueDateInputDocument { get; set; }

    // Ответственный сотрудник
    public int? IdResponsibleEmployeeInputDocument { get; set; }
    public string? ResponsibleEmployeeFullName { get; set; }

    // Выходные данные документа
    public bool IsExternalDocumentOutputDocument { get; set; }
    public string? OutgoingDocumentNumberOutputDocument { get; set; }
    public DateOnly? OutgoingDocumentDateOutputDocument { get; set; }
    public string? RecipientOutputDocument { get; set; }
    public string? DocumentSummaryOutputDocument { get; set; }

    // Статусы
    public bool IsUnderControl { get; set; }
    public bool IsCompleted { get; set; }
}
