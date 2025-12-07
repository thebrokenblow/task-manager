using TaskManager.Domain.Enums;
using TaskManager.Domain.Model.Documents;

namespace TaskManager.Domain.Queries;

public interface IDocumentQuery
{
    Task<(List<DocumentForOverviewModel> documents, int countDocuments)> GetDocumentsAsync(
        string? searchTerm,
        int countSkip,
        int countTake,
        DocumentStatus documentStatus);

    Task<DocumentForEditModel?> GetDocumentForEditAsync(int id);

    Task<DocumentForDeleteModel?> GetDocumentForDeleteAsync(int id);

    Task<DocumentForChangeStatusModel?> GetDocumentForChangeStatusAsync(int id);

    Task<DocumentForCsvExportModel?> GetDocumentForCsvExportAsync(int id);

    Task<int?> GetIdEmployeeCreatedAsync(int id);

    Task<int?> GetIdEmployeeRemovedAsync(int id);
}