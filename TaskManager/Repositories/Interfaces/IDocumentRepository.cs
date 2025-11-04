using TaskManager.Models;

namespace TaskManager.Repositories.Interfaces;

public interface IDocumentRepository
{
    Task AddAsync(Document document);
    Task UpdateAsync(Document document);
    Task RemoveAsync(Document document);
    Task<Document?> GetByIdAsync(int id);
    Task ChangeAuthorAsync(Document document, string user);
    Task RecoverDeletedTaskAsync(Document document);
}