using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Model.Documents;
using TaskManager.Domain.Queries;
using TaskManager.Persistence.Data;

namespace TaskManager.Persistence.Queries;

public class DocumentQuery(TaskManagerDbContext context) : IDocumentQuery
{
    public async Task<(List<DocumentForOverviewModel> documents, int countDocuments)> GetDocumentsAsync(int countSkip, int countTake)
    {
        var queryDocuments = context.Documents.Include(document => document.ResponsibleEmployeeInputDocument)
                                              .Where(document => document.IsCompleted == false && document.RemoveDateTime == null)
                                              .AsQueryable();

        var countDocuments = await queryDocuments.CountAsync();

        var documents = await queryDocuments.OrderBy(document => document.TaskDueDateInputDocument)
                                            .ThenBy(document => document.IsUnderControl)
                                            .Skip(countSkip)
                                            .Take(countTake)
                                            .Select(document => new DocumentForOverviewModel
                                            {
                                                Id = document.Id,
                                                OutgoingDocumentNumberInputDocument = document.OutgoingDocumentNumberInputDocument,
                                                DocumentSummaryInputDocument = document.DocumentSummaryInputDocument,
                                                IncomingDocumentNumberInputDocument = document.IncomingDocumentNumberInputDocument,
                                                CustomerInputDocument = document.CustomerInputDocument,
                                                TaskDueDateInputDocument = document.TaskDueDateInputDocument,
                                                OutgoingDocumentNumberOutputDocument = document.OutgoingDocumentNumberOutputDocument,
                                                OutgoingDocumentDateOutputDocument = document.OutgoingDocumentDateOutputDocument,
                                                CreatedByEmployeeId = document.CreatedByEmployeeId,

                                                FullNameResponsibleEmployee =
                                                    document.ResponsibleEmployeeInputDocument != null
                                                    ? document.ResponsibleEmployeeInputDocument.FullName
                                                    : null,

                                                RemovedByEmployeeId = document.RemovedByEmployeeId,
                                                RemoveDateTime = document.RemoveDateTime,

                                                IsCompleted = document.IsCompleted,
                                                IsUnderControl = document.IsUnderControl,
                                            })
                                            .AsNoTracking()
                                            .ToListAsync();

        return (documents, countDocuments);
    }

    public async Task<(List<DocumentForOverviewModel> documents, int countDocuments)> SearchDocumentsAsync(
        string inputSearch,
        int countSkip,
        int countTake)
    {
        var queryDocuments = context.Documents.Include(document => document.ResponsibleEmployeeInputDocument)
                                              .AsQueryable();

        if (!string.IsNullOrWhiteSpace(inputSearch))
        {
            queryDocuments = queryDocuments.Where(document =>
                                        (document.OutgoingDocumentNumberInputDocument != null && document.OutgoingDocumentNumberInputDocument.Contains(inputSearch)) ||
                                        (document.DocumentSummaryInputDocument != null && document.DocumentSummaryInputDocument.Contains(inputSearch)) ||
                                        (document.IncomingDocumentNumberInputDocument != null && document.IncomingDocumentNumberInputDocument.Contains(inputSearch)) ||
                                        (document.OutgoingDocumentNumberOutputDocument != null && document.OutgoingDocumentNumberOutputDocument.Contains(inputSearch)));
        }

        var countDocuments = await queryDocuments.CountAsync();

        var documents = await queryDocuments.OrderBy(document => document.TaskDueDateInputDocument)
                                            .ThenBy(document => document.IsUnderControl)
                                            .Select(document => new DocumentForOverviewModel
                                            {
                                                Id = document.Id,
                                                OutgoingDocumentNumberInputDocument = document.OutgoingDocumentNumberInputDocument,
                                                DocumentSummaryInputDocument = document.DocumentSummaryInputDocument,
                                                IncomingDocumentNumberInputDocument = document.IncomingDocumentNumberInputDocument,
                                                CustomerInputDocument = document.CustomerInputDocument,
                                                TaskDueDateInputDocument = document.TaskDueDateInputDocument,
                                                OutgoingDocumentNumberOutputDocument = document.OutgoingDocumentNumberOutputDocument,
                                                OutgoingDocumentDateOutputDocument = document.OutgoingDocumentDateOutputDocument,
                                                CreatedByEmployeeId = document.CreatedByEmployeeId,

                                                FullNameResponsibleEmployee =
                                                    document.ResponsibleEmployeeInputDocument != null ? 
                                                    document.ResponsibleEmployeeInputDocument.FullName : 
                                                    null,

                                                RemovedByEmployeeId = document.RemovedByEmployeeId,
                                                RemoveDateTime = document.RemoveDateTime,
                                                IsCompleted = document.IsCompleted,
                                                IsUnderControl = document.IsUnderControl,
                                            })
                                            .AsNoTracking()
                                            .Skip(countSkip)
                                            .Take(countTake)
                                            .ToListAsync();

        return (documents, countDocuments);
    }

    public async Task<DocumentForEditModel?> GetDocumentForEditAsync(int id)
    {
        var document = await context.Documents
                                    .Select(document => new DocumentForEditModel
                                    {
                                        Id = document.Id,
                                        OutgoingDocumentNumberInputDocument = document.OutgoingDocumentNumberInputDocument,
                                        SourceDocumentDateInputDocument = document.SourceDocumentDateInputDocument,
                                        CustomerInputDocument = document.CustomerInputDocument,
                                        DocumentSummaryInputDocument = document.DocumentSummaryInputDocument,
                                        IsExternalDocumentInputDocument = document.IsExternalDocumentInputDocument,
                                        IncomingDocumentNumberInputDocument = document.IncomingDocumentNumberInputDocument,
                                        IncomingDocumentDateInputDocument = document.IncomingDocumentDateInputDocument,
                                        ResponsibleDepartmentsInputDocument = document.ResponsibleDepartmentsInputDocument,
                                        TaskDueDateInputDocument = document.TaskDueDateInputDocument,
                                        IdResponsibleEmployeeInputDocument = document.IdResponsibleEmployeeInputDocument,
                                        IsExternalDocumentOutputDocument = document.IsExternalDocumentOutputDocument,
                                        OutgoingDocumentNumberOutputDocument = document.OutgoingDocumentNumberOutputDocument,
                                        OutgoingDocumentDateOutputDocument = document.OutgoingDocumentDateOutputDocument,
                                        RecipientOutputDocument = document.RecipientOutputDocument,
                                        DocumentSummaryOutputDocument = document.DocumentSummaryOutputDocument,
                                        IsUnderControl = document.IsUnderControl,
                                        IsCompleted = document.IsCompleted,

                                        FullNameLastEditedEmployee =
                                                    document.LastEditedByEmployee != null ? 
                                                    document.LastEditedByEmployee.FullName : 
                                                    null,

                                        RemovedByEmployeeId = document.RemovedByEmployeeId,
                                        RemoveDateTime = document.RemoveDateTime,
                                        LastEditedDateTime = document.LastEditedDateTime,
                                        CreatedByEmployeeId = document.CreatedByEmployeeId,
                                    })
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(document => document.Id == id);

        return document;
    }

    public async Task<DocumentForDeleteModel?> GetDocumentForDeleteAsync(int id)
    {
        var document = await context.Documents
                                    .Select(document => new DocumentForDeleteModel
                                    {
                                        Id = document.Id,
                                        OutgoingDocumentNumberInputDocument = document.OutgoingDocumentNumberInputDocument,
                                        SourceDocumentDateInputDocument = document.SourceDocumentDateInputDocument,
                                        CustomerInputDocument = document.CustomerInputDocument,
                                        DocumentSummaryInputDocument = document.DocumentSummaryInputDocument,
                                        IsExternalDocumentInputDocument = document.IsExternalDocumentInputDocument,
                                        IncomingDocumentNumberInputDocument = document.IncomingDocumentNumberInputDocument,
                                        IncomingDocumentDateInputDocument = document.IncomingDocumentDateInputDocument,
                                        ResponsibleDepartmentsInputDocument = document.ResponsibleDepartmentsInputDocument,
                                        TaskDueDateInputDocument = document.TaskDueDateInputDocument,
                                        IdResponsibleEmployeeInputDocument = document.IdResponsibleEmployeeInputDocument,

                                        ResponsibleEmployeeFullName = 
                                                    document.ResponsibleEmployeeInputDocument != null ? 
                                                    document.ResponsibleEmployeeInputDocument.FullName : 
                                                    null,
                                        
                                        IsExternalDocumentOutputDocument = document.IsExternalDocumentOutputDocument,
                                        OutgoingDocumentNumberOutputDocument = document.OutgoingDocumentNumberOutputDocument,
                                        OutgoingDocumentDateOutputDocument = document.OutgoingDocumentDateOutputDocument,
                                        RecipientOutputDocument = document.RecipientOutputDocument,
                                        DocumentSummaryOutputDocument = document.DocumentSummaryOutputDocument,
                                        IsUnderControl = document.IsUnderControl,
                                        IsCompleted = document.IsCompleted
                                    })
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(document => document.Id == id);

        return document;
    }

    public async Task<int?> GetIdEmployeeRemovedAsync(int id)
    {
        var removedByEmployeeId = await context.Documents.Where(document => document.Id == id)
                                                         .Select(document => document.RemovedByEmployeeId)
                                                         .FirstOrDefaultAsync();

        return removedByEmployeeId;
    }

    public async Task<DocumentForChangeStatusModel?> GetDocumentForChangeStatusAsync(int id)
    {
        var documentForChangeStatusModel = await context.Documents.Where(document => document.Id == id)
                                                                  .Select(document => new DocumentForChangeStatusModel
                                                                  { 
                                                                       DocumentSummaryOutputDocument = document.DocumentSummaryOutputDocument,
                                                                       OutgoingDocumentDateOutputDocument = document.OutgoingDocumentDateOutputDocument,
                                                                       OutgoingDocumentNumberOutputDocument = document.OutgoingDocumentNumberOutputDocument,
                                                                       RecipientOutputDocument = document.RecipientOutputDocument,
                                                                       IsCompleted = document.IsCompleted
                                                                  })
                                                                  .FirstOrDefaultAsync();

        return documentForChangeStatusModel;
    }
}