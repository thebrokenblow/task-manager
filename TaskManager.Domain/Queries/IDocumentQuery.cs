using TaskManager.Domain.Entities;
using TaskManager.Domain.Model.Documents;

namespace TaskManager.Domain.Queries;

public interface IDocumentQuery
{
    Task<(List<DocumentForOverviewModel> documents, int countDocuments)> GetDocumentsAsync(
        int countSkip,
        int countTake);

    Task<(List<DocumentForOverviewModel> documents, int countDocuments)> SearchDocumentsAsync(
        string searchTerm,
        int countSkip,
        int countTake);

    Task<DocumentForEditModel?> GetDocumentForEditAsync(int id);

    Task<DocumentForDeleteModel?> GetDocumentForDeleteAsync(int id);

    Task<int?> GetIdEmployeeRemovedAsync(int id);

    Task<DocumentForChangeStatusModel?> GetDocumentForChangeStatusAsync(int id);
}