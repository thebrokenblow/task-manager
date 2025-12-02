using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Repositories;

public interface IDocumentRepository
{
    Task<Document?> GetByIdAsync(int id);

    Task AddAsync(Document document);
    Task UpdateAsync(Document document);
    Task RemoveHardAsync(int id);
    Task RemoveSoftAsync(int id, int idEmployeeRemove, int idAdmin, DateTime removeDateTime);
    Task RecoverDeletedAsync(int id, int idEmployeeRemove);
    Task UpdateStatusAsync(int id, bool isCompleted);
}