using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Exceptions;
using TaskManager.Domain.Repositories;
using TaskManager.Persistence.Data;

namespace TaskManager.Persistence.Repositories;

public class DocumentRepository(TaskManagerDbContext context) : IDocumentRepository
{
    public async Task<Document?> GetByIdAsync(int id)
    {
        var document = await context.Documents.FindAsync(id);

        return document;
    }

    public async Task AddAsync(Document document)
    {
        await context.AddAsync(document);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Document document)
    {
        context.Update(document);
        await context.SaveChangesAsync();
    }

    public async Task RemoveHardAsync(int id)
    {
        var affectedRows = await context.Documents
                                        .Where(document => document.Id == id)
                                        .ExecuteDeleteAsync();

        if (affectedRows == 0)
        {
            throw new NotFoundException(nameof(Document), id);
        }
    }

    public async Task RemoveSoftAsync(int id, int idEmployeeRemove, int idAdmin, DateTime removeDateTime)
    {
        var affectedRows = await context.Documents
            .Where(document => document.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(document => document.RemovedByEmployeeId, idEmployeeRemove)
                .SetProperty(document => document.RemoveDateTime, removeDateTime) 
                .SetProperty(document => document.CreatedByEmployeeId, idAdmin)
            );

        if (affectedRows == 0)
        {
            throw new NotFoundException(nameof(Document), id);
        }
    }

    public async Task RecoverDeletedAsync(int id, int idEmployeeRemove)
    {
        var affectedRows = await context.Documents
            .Where(document => document.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(document => document.CreatedByEmployeeId, idEmployeeRemove)
                .SetProperty(document => document.RemovedByEmployeeId, (int?)null)
                .SetProperty(document => document.RemoveDateTime, (DateTime?)null)
            );

        if (affectedRows == 0)
        {
            throw new NotFoundException(nameof(Document), id);
        }
    }

    public async Task UpdateStatusAsync(int id, bool isCompleted)
    {
        var affectedRows = await context.Documents
            .Where(document => document.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(document => document.IsCompleted, isCompleted)
            );

        if (affectedRows == 0)
        {
            throw new NotFoundException(nameof(Document), id);
        }
    }
}