using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.Queries.Interfaces;

namespace TaskManager.Queries;

public class DocumentQuery(TaskManagerDbContext context) : IDocumentQuery
{
    public async Task<(List<FilteredRangeDocument> documents, int countDocuments)> GetFilteredRangeAsync(
        string inputSearch, 
        int countSkip, 
        int countTake)
    {
        var queryDocuments = context.Documents.Include(document => document.SourceResponsibleEmployee)
                                              .AsQueryable();

        if (!string.IsNullOrWhiteSpace(inputSearch))
        {
            queryDocuments = queryDocuments.Where(document => 
                                        document.SourceOutgoingDocumentNumber.Contains(inputSearch) ||
                                        document.SourceOutputDocumentNumber.Contains(inputSearch) ||
                                        document.SourceTaskText.Contains(inputSearch));
        }

        var countDocuments = await queryDocuments.CountAsync();

        var documents = await queryDocuments.Select(document => new FilteredRangeDocument
                                            {
                                                Id = document.Id,
                                                SourceOutgoingDocumentNumber = document.SourceOutgoingDocumentNumber,
                                                SourceTaskText = document.SourceTaskText,
                                                SourceOutputDocumentNumber = document.SourceOutputDocumentNumber,
                                                SourceDueDate = document.SourceDueDate,
                                                OutputOutgoingDate = document.OutputOutgoingDate,
                                                OutputOutgoingNumber = document.OutputOutgoingNumber,
                                                SourceResponsibleEmployeeId = document.SourceResponsibleEmployeeId,
                                                SourceResponsibleEmployee = document.SourceResponsibleEmployee!
                                            })
                                            .AsNoTracking()
                                            .OrderBy(document => document.SourceOutgoingDocumentNumber)
                                            .ThenBy(document => document.SourceOutputDocumentNumber)
                                            .ThenBy(document => document.SourceDueDate)
                                            .ThenBy(document => document.SourceResponsibleEmployee.FullName)
                                            .Skip(countSkip)
                                            .Take(countTake)
                                            .ToListAsync();

        return (documents, countDocuments);
    }

    public async Task<Document?> GetDetailsByIdAsync(int id)
    {
        var document = await context.Documents.Include(document => document.SourceResponsibleEmployee)
                                              .FirstOrDefaultAsync(document => document.Id == id);

        return document;
    }
}