using Microsoft.EntityFrameworkCore;
using TaskManager.Models;
using TaskManager.Repositories.Interfaces;
using TaskManager.View.Data;

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
}