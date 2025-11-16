using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces.Queries;
using TaskManager.Domain.Models;
using TaskManager.Persistence.Data;

namespace TaskManager.Persistence.Queries;

public class DocumentQuery(TaskManagerDbContext context) : IDocumentQuery
{
    public async Task<(List<DocumentOverviewModel> documents, int countDocuments)> GetActiveDocumentsAsync(
        int countSkip,
        int countTake)
    {
        var queryDocuments = context.Documents.Include(document => document.AuthorCreateDocument)
                                              .AsQueryable();

        queryDocuments = queryDocuments.Where(document => !document.IsCompleted &&
                                                           document.IdAuthorRemoveDocument == null &&
                                                           document.DateRemove == null);

        var countDocuments = await queryDocuments.CountAsync();

        var documents = await queryDocuments.OrderBy(document => document.SourceDueDate)
                                            .ThenBy(document => document.IsUnderControl)
                                            .Select(document => new DocumentOverviewModel
                                            {
                                                Id = document.Id,
                                                SourceOutgoingDocumentNumber = document.SourceOutgoingDocumentNumber,
                                                SourceTaskText = document.SourceTaskText,
                                                SourceOutputDocumentNumber = document.SourceOutputDocumentNumber,
                                                SourceDueDate = document.SourceDueDate,
                                                IdAuthorCreateDocument = document.IdAuthorCreateDocument,
                                                AuthorCreateDocument = document.AuthorCreateDocument!,
                                                OutputOutgoingNumber = document.OutputOutgoingNumber,
                                                OutputOutgoingDate = document.OutputOutgoingDate,
                                                IsUnderControl = document.IsUnderControl,
                                                IsCompleted = document.IsCompleted,
                                                IdAuthorRemoveDocument = document.IdAuthorRemoveDocument,
                                                DateRemove = document.DateRemove,
                                            })
                                            .AsNoTracking()
                                            .Skip(countSkip)
                                            .Take(countTake)
                                            .ToListAsync();

        return (documents, countDocuments);
    }

    public async Task<(List<DocumentOverviewModel> documents, int countDocuments)> SearchDocumentsAsync(
        string searchTerm,
        int countSkip,
        int countTake)
    {
        var queryDocuments = context.Documents.Include(document => document.AuthorCreateDocument)
                                              .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            queryDocuments = queryDocuments.Where(document =>
                                        document.SourceOutgoingDocumentNumber.Contains(searchTerm) ||
                                        document.SourceOutputDocumentNumber.Contains(searchTerm) ||
                                        document.SourceTaskText.Contains(searchTerm) ||
                                        document.OutputOutgoingNumber != null && document.OutputOutgoingNumber.Contains(searchTerm));
        }

        var countDocuments = await queryDocuments.CountAsync();

        var documents = await queryDocuments.OrderBy(document => document.SourceDueDate)
                                            .ThenBy(document => document.IsUnderControl)
                                            .Select(document => new DocumentOverviewModel
                                            {
                                                Id = document.Id,
                                                SourceOutgoingDocumentNumber = document.SourceOutgoingDocumentNumber,
                                                SourceTaskText = document.SourceTaskText,
                                                SourceOutputDocumentNumber = document.SourceOutputDocumentNumber,
                                                SourceDueDate = document.SourceDueDate,
                                                IsUnderControl = document.IsUnderControl,
                                                OutputOutgoingDate = document.OutputOutgoingDate,
                                                IdAuthorCreateDocument = document.IdAuthorCreateDocument,
                                                AuthorCreateDocument = document.AuthorCreateDocument!,
                                                OutputOutgoingNumber = document.OutputOutgoingNumber,
                                                IdAuthorRemoveDocument = document.IdAuthorRemoveDocument,
                                                IsCompleted = document.IsCompleted,
                                                DateRemove = document.DateRemove,
                                            })
                                            .AsNoTracking()
                                            .Skip(countSkip)
                                            .Take(countTake)
                                            .ToListAsync();

        return (documents, countDocuments);
    }

    public async Task<Document?> GetDocumentByIdAsync(int id)
    {
        var document = await context.Documents.Include(document => document.AuthorCreateDocument)
                                              .Include(document => document.AuthorRemoveDocument)
                                              .FirstOrDefaultAsync(document => document.Id == id);

        return document;
    }
}