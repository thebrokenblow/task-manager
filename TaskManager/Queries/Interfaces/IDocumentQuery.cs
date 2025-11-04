using TaskManager.Models;

namespace TaskManager.Queries.Interfaces;

public interface IDocumentQuery
{
    Task<(List<FilteredRangeDocument> documents, int countDocuments)> GetRangeAsync(
        int countSkip,
        int countTake);

    Task<(List<FilteredRangeDocument> documents, int countDocuments)> GetFilteredRangeAsync(
        string inputSearch, 
        int countSkip, 
        int countTake);

    Task<Document?> GetDetailsByIdAsync(int id);
}
