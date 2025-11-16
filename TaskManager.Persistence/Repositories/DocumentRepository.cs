using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces.Repositories;
using TaskManager.Persistence.Data;

namespace TaskManager.Persistence.Repositories;

public class DocumentRepository(TaskManagerDbContext context) : IDocumentRepository
{
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

    public async Task RemoveAsync(Document document)
    {
        context.Remove(document);
        await context.SaveChangesAsync();
    }

    public Task<Document?> GetByIdAsync(int id)
    {
        var document = context.Documents.FirstOrDefaultAsync(document => document.Id == id);

        return document;
    }

    public async Task ChangeAuthorAsync(Document document, int id)
    {
        document.IdAuthorRemoveDocument = document.IdAuthorCreateDocument;
        document.IdAuthorCreateDocument = id;

        document.DateRemove = DateTime.UtcNow;

        context.Update(document);
        await context.SaveChangesAsync();
    }

    public async Task RecoverDeletedTaskAsync(Document document)
    {
        document.DateRemove = null;
        document.AuthorCreateDocument = document.AuthorRemoveDocument!;

        context.Update(document);
        await context.SaveChangesAsync();
    }
}