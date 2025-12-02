namespace TaskManager.Domain.Model.Documents;

public class DocumentForChangeStatusModel
{
    public required string? OutgoingDocumentNumberOutputDocument { get; init; }
    public required DateOnly? OutgoingDocumentDateOutputDocument { get; init; }
    public required string? RecipientOutputDocument { get; init; }
    public required string? DocumentSummaryOutputDocument { get; init; }
    public required bool IsCompleted { get; init; }
}