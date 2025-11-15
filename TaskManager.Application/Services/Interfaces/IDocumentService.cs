using TaskManager.Application.Common;
using TaskManager.Domain.Entities;
using TaskManager.Domain.QueryModels;

namespace TaskManager.Application.Services.Interfaces;

public interface IDocumentService
{
    Task<Document?> GetDetailsByIdAsync(int id);
    Task<PagedResult<FilteredRangeDocumentModel>> GetFilteredRangeAsync(string inputSearch, int page, int pageSize); 
    Task CreateAsync(Document document);
    Task EditAsync(Document document);
    Task DeleteAsync(int id);
    Task RecoverDeletedDocumentAsync(int id);
    Task ChangeStatusDocumentAsync(int id);
}