using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces.Repositories;

public interface IDocumentRepository
{
    Task<Document?> GetDetailsByIdAsync(int id);

    Task AddAsync(Document document);
    Task UpdateAsync(Document document);
    Task RemoveAsync(Document document);
    Task<Document?> GetByIdAsync(int id);
    Task ChangeAuthorAsync(Document document, int id);
    Task RecoverDeletedTaskAsync(Document document);
}