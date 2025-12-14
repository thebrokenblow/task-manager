using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Model.Documents;
using TaskManager.Domain.Queries;
using TaskManager.Persistence.Data;
using TaskManager.Persistence.Extensions;

namespace TaskManager.Persistence.Queries;

/// <summary>
/// Запросы для работы с документами.
/// </summary>
public class DocumentQuery(TaskManagerDbContext context) : IDocumentQuery
{
    /// <summary>
    /// Получает список документов с фильтрацией и пагинацией.
    /// </summary>
    /// <param name="documentFilterModel">Модель фильтрации документов.</param>
    /// <param name="countSkip">Количество пропущенных записей.</param>
    /// <param name="countTake">Количество получаемых записей.</param>
    public async Task<(List<DocumentForOverviewModel> documents, int countDocuments)> GetDocumentsAsync(
        DocumentFilterModel documentFilterModel,
        int countSkip,
        int countTake)
    {
        var queryDocuments = context.Documents.Include(document => document.ResponsibleEmployeeInputDocument)
                                              .AsQueryable();

        queryDocuments = queryDocuments.FilterByOutputDate(documentFilterModel)
                                       .FilterByResponsibleEmployee(documentFilterModel);

        if (!string.IsNullOrWhiteSpace(documentFilterModel.SearchTerm) ||
            documentFilterModel.StartOutgoingDocumentDateOutputDocument.HasValue ||
            documentFilterModel.EndOutgoingDocumentDateOutputDocument.HasValue)
        {
            queryDocuments = queryDocuments.FilterByDocumentSearchTerm(documentFilterModel.SearchTerm)
                                           .FilterByOutputDate(documentFilterModel);
        }
        else
        {
            queryDocuments = queryDocuments.FilterDocumentStatus(documentFilterModel);
        }

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

    /// <summary>
    /// Получает данные документа для редактирования.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
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
                                        ResponsibleDepartmentInputDocument = document.ResponsibleDepartmentInputDocument,
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

    /// <summary>
    /// Получает данные документа для удаления.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
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

    /// <summary>
    /// Получает идентификатор сотрудника, удалившего документ.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    public async Task<int?> GetIdEmployeeRemovedAsync(int id)
    {
        var removedByEmployeeId = await context.Documents.Where(document => document.Id == id)
                                                         .Select(document => document.RemovedByEmployeeId)
                                                         .FirstOrDefaultAsync();

        return removedByEmployeeId;
    }

    /// <summary>
    /// Получает идентификатор сотрудника, создавшего документ.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    public async Task<int?> GetIdEmployeeCreatedAsync(int id)
    {
        var isExist = await context.Documents.AnyAsync(document => document.Id == id);

        if (!isExist)
        {
            return null;
        }

        var createdByEmployeeId = await context.Documents.Where(document => document.Id == id)
                                                         .Select(document => document.CreatedByEmployeeId)
                                                         .FirstOrDefaultAsync();

        return createdByEmployeeId;
    }

    /// <summary>
    /// Получает данные документа для изменения статуса.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
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

    /// <summary>
    /// Получает данные документа для экспорта в CSV.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    public async Task<DocumentForCsvExportModel?> GetDocumentForCsvExportAsync(int id)
    {
        var documentForExportModel = await context.Documents.Where(document => document.Id == id)
                                                            .Select(document => new DocumentForCsvExportModel
                                                            {
                                                                OutgoingDocumentNumberInputDocument = document.OutgoingDocumentNumberInputDocument,
                                                                SourceDocumentDateInputDocument = document.SourceDocumentDateInputDocument,
                                                                CustomerInputDocument = document.CustomerInputDocument,
                                                                DocumentSummaryInputDocument = document.DocumentSummaryInputDocument,
                                                                IsExternalDocumentInputDocument = document.IsExternalDocumentInputDocument,
                                                                IncomingDocumentNumberInputDocument = document.IncomingDocumentNumberInputDocument,
                                                                IncomingDocumentDateInputDocument = document.IncomingDocumentDateInputDocument,
                                                                ResponsibleDepartmentsInputDocument = document.ResponsibleDepartmentsInputDocument,
                                                                TaskDueDateInputDocument = document.TaskDueDateInputDocument,

                                                                FullNameResponsibleEmployeeInputDocument = document.ResponsibleEmployeeInputDocument != null
                                                                    ? document.ResponsibleEmployeeInputDocument.FullName
                                                                    : null,

                                                                IsExternalDocumentOutputDocument = document.IsExternalDocumentOutputDocument,
                                                                OutgoingDocumentNumberOutputDocument = document.OutgoingDocumentNumberOutputDocument,
                                                                OutgoingDocumentDateOutputDocument = document.OutgoingDocumentDateOutputDocument,
                                                                RecipientOutputDocument = document.RecipientOutputDocument,
                                                                DocumentSummaryOutputDocument = document.DocumentSummaryOutputDocument,

                                                                IsUnderControl = document.IsUnderControl,
                                                                IsCompleted = document.IsCompleted,

                                                                FullNameCreatedEmployee = document.CreatedByEmployee != null
                                                                    ? document.CreatedByEmployee.FullName
                                                                    : string.Empty,
                                                            })
                                                            .FirstOrDefaultAsync();

        return documentForExportModel;
    }
}