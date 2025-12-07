using TaskManager.Application.Common;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Model.Documents;

namespace TaskManager.Application.Services.Interfaces;

public interface IDocumentService
{
    Task<PagedResult<DocumentForOverviewModel>> GetPagedAsync(string inputSearch, int page, int pageSize);

    Task<Document?> GetByIdAsync(int id);

    Task<DocumentForEditModel?> GetDocumentForEditAsync(int id);

    Task<DocumentForDeleteModel?> GetDocumentForDeleteAsync(int id);

    Task CreateAsync(Document document);

    Task EditAsync(Document document);

    Task DeleteAsync(int id);

    Task RecoverDeletedAsync(int id);

    Task ChangeStatusAsync(int id);

    Task<byte[]> CreateDocumentCsvAsync(int id);
}