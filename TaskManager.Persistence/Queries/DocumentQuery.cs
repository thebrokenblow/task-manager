using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces.Queries;
using TaskManager.Domain.QueryModels;
using TaskManager.Persistence.Data;

namespace TaskManager.Persistence.Queries;

public class DocumentQuery(TaskManagerDbContext context) : IDocumentQuery
{
    public async Task<(List<FilteredRangeDocumentModel> documents, int countDocuments)> GetRangeAsync(int countSkip, int countTake)
    {
        var queryDocuments = context.Documents.Include(document => document.AuthorCreateDocument)
                                              .AsQueryable();

        queryDocuments = queryDocuments.Skip(countSkip)
                                       .Take(countTake)
                                       .Where(document => !document.IsCompleted && document.DateRemove == null);

        var countDocuments = await queryDocuments.CountAsync();

        var documents = await queryDocuments.OrderBy(document => document.SourceDueDate)
                                            .ThenBy(document => document.IsUnderControl)
                                            .Select(document => new FilteredRangeDocumentModel
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
                                            .ToListAsync();

        return (documents, countDocuments);
    }

    public async Task<(List<FilteredRangeDocumentModel> documents, int countDocuments)> GetFilteredRangeAsync(
        string inputSearch, 
        int countSkip, 
        int countTake)
    {
        var queryDocuments = context.Documents.Include(document => document.AuthorCreateDocument)
                                              .AsQueryable();

        if (!string.IsNullOrWhiteSpace(inputSearch))
        {
            queryDocuments = queryDocuments.Where(document => 
                                        document.SourceOutgoingDocumentNumber.Contains(inputSearch) ||
                                        document.SourceOutputDocumentNumber.Contains(inputSearch) ||
                                        document.SourceTaskText.Contains(inputSearch) ||
                                        document.OutputOutgoingNumber != null && document.OutputOutgoingNumber.Contains(inputSearch));
        }

        var countDocuments = await queryDocuments.CountAsync();

        var documents = await queryDocuments.OrderBy(document => document.SourceDueDate)
                                            .ThenBy(document => document.IsUnderControl)
                                            .Select(document => new FilteredRangeDocumentModel
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

    public async Task<Document?> GetDetailsByIdAsync(int id)
    {
        var document = await context.Documents.Include(document => document.AuthorCreateDocument)
                                              .Include(document => document.AuthorRemoveDocument)
                                              .FirstOrDefaultAsync(document => document.Id == id);

        return document;
    }

    public async Task<(List<FilteredRangeDocumentModel> documents, int countDocuments)> GetDeletedRangeAsync(
        string inputSearch, 
        int countSkip, 
        int countTake)
    {
        var queryDocuments = context.Documents.Include(document => document.AuthorCreateDocument)
                                              .AsQueryable();

        if (!string.IsNullOrWhiteSpace(inputSearch))
        {
            queryDocuments = queryDocuments.Where(document =>
                                        document.SourceOutgoingDocumentNumber.Contains(inputSearch) ||
                                        document.SourceOutputDocumentNumber.Contains(inputSearch) ||
                                        document.SourceTaskText.Contains(inputSearch) ||
                                        document.OutputOutgoingNumber != null && document.OutputOutgoingNumber.Contains(inputSearch));
        }

        var countDocuments = await queryDocuments.CountAsync();

        var documents = await queryDocuments.OrderBy(document => document.SourceDueDate)
                                            .ThenBy(document => document.IsUnderControl)
                                            .Select(document => new FilteredRangeDocumentModel
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
}