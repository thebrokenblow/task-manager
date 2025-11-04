using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Repositories.Interfaces;

namespace TaskManager.Repositories;

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

    public async Task ChangeAuthorAsync(Document document, string user)
    {
        document.AuthorRemoveDocument = document.LoginAuthor;
        document.LoginAuthor = user;

        document.DateRemove = DateTime.UtcNow;

        context.Update(document);
        await context.SaveChangesAsync();
    }

    public async Task RecoverDeletedTaskAsync(Document document)
    {
        document.DateRemove = null;
        document.LoginAuthor = document.AuthorRemoveDocument!;

        context.Update(document);
        await context.SaveChangesAsync();
    }
}