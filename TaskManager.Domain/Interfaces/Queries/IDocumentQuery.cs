using TaskManager.Domain.QueryModels;

namespace TaskManager.Domain.Interfaces.Queries;

public interface IDocumentQuery
{
    Task<(List<FilteredRangeDocumentModel> documents, int countDocuments)> GetRangeAsync(
        int countSkip,
        int countTake);

    Task<(List<FilteredRangeDocumentModel> documents, int countDocuments)> GetDeletedRangeAsync(
        string inputSearch,
        int countSkip,
        int countTake);

    Task<(List<FilteredRangeDocumentModel> documents, int countDocuments)> GetFilteredRangeAsync(
        string inputSearch, 
        int countSkip, 
        int countTake);
}