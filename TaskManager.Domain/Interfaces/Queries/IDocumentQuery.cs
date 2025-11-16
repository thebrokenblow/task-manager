using TaskManager.Domain.Entities;
using TaskManager.Domain.Models;

namespace TaskManager.Domain.Interfaces.Queries;

public interface IDocumentQuery
{
    Task<(List<DocumentOverviewModel> documents, int countDocuments)> GetActiveDocumentsAsync(
        int countSkip,
        int countTake);

    Task<(List<DocumentOverviewModel> documents, int countDocuments)> SearchDocumentsAsync(
        string searchTerm,
        int countSkip,
        int countTake);

    Task<Document?> GetDocumentByIdAsync(int id);
}