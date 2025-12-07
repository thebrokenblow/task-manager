using FluentValidation;
using TaskManager.Application.Common;
using TaskManager.Application.Exceptions;
using TaskManager.Application.Services.Interfaces;
using TaskManager.Application.Validations;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Exceptions;
using TaskManager.Domain.Model.Documents;
using TaskManager.Domain.Queries;
using TaskManager.Domain.Repositories;
using TaskManager.Domain.Services;

namespace TaskManager.Application.Services;

public class DocumentService(
    IDocumentQuery documentQuery, 
    IDocumentRepository documentRepository,
    IAuthService authService,
    IExportService exportService) : IDocumentService
{
    public async Task<PagedResult<DocumentForOverviewModel>> GetPagedAsync(string inputSearch, int page, int pageSize)
    {
        int countDocuments;
        List<DocumentForOverviewModel> documents;

        int countSkip = (page - 1) * pageSize;

        if (authService.IsAdmin)
        {
            (documents, countDocuments) = await documentQuery.GetDocumentsAsync(inputSearch, countSkip, pageSize, DocumentStatus.Archived);
        }
        else
        {
            (documents, countDocuments) = await documentQuery.GetDocumentsAsync(inputSearch, countSkip, pageSize, DocumentStatus.Active);
        }

        var pagedResult = new PagedResult<DocumentForOverviewModel>(documents, countDocuments, page, pageSize);

        return pagedResult;
    }

    public async Task<DocumentForEditModel?> GetDocumentForEditAsync(int id)
    {
        var document = await documentQuery.GetDocumentForEditAsync(id);

        return document;
    }

    public async Task<DocumentForDeleteModel?> GetDocumentForDeleteAsync(int id)
    {
        var document = await documentQuery.GetDocumentForDeleteAsync(id);

        return document;
    }

    public async Task<Document?> GetByIdAsync(int id)
    {
        var document = await documentRepository.GetByIdAsync(id);

        return document;
    }

    public async Task ChangeStatusAsync(int id)
    {
        var documentForChangeStatusModel = await documentQuery.GetDocumentForChangeStatusAsync(id) ?? 
                                                    throw new NotFoundException(nameof(DocumentForChangeStatusModel), id);

        var documentForChangeStatusModelValidator = new DocumentForChangeStatusModelValidator();
        var validationResult = documentForChangeStatusModelValidator.Validate(documentForChangeStatusModel);

        if (!validationResult.IsValid)
        {
            throw new IncompleteOutputDocumentException();
        }

        await documentRepository.UpdateStatusAsync(id, !documentForChangeStatusModel.IsCompleted);
    }

    public async Task CreateAsync(Document document)
    {
        if (!authService.IsAuthenticated || !authService.IdCurrentUser.HasValue)
        {
            throw new UnauthorizedAccessException("Пользователь не аутентифицирован");
        }

        TrimDocumentProperties(document);

        document.CreatedByEmployeeId = authService.IdCurrentUser.Value;
        await documentRepository.AddAsync(document);
    }

    public async Task EditAsync(Document document)
    {
        TrimDocumentProperties(document);

        document.LastEditedDateTime = DateTime.Now;
        document.LastEditedByEmployeeId = authService.IdCurrentUser;

        await documentRepository.UpdateAsync(document);
    }

    public async Task RecoverDeletedAsync(int id)
    {
        var removedByEmployeeId = await documentQuery.GetIdEmployeeRemovedAsync(id) ?? 
            throw new InvalidOperationException($"Id сотрудника который удалил запись равна null, у документа с id: {id}");

        await documentRepository.RecoverDeletedAsync(id, removedByEmployeeId);
    }

    public async Task DeleteAsync(int id)
    {
        if (!authService.IsAuthenticated || !authService.IdCurrentUser.HasValue)
        {
            throw new UnauthorizedAccessException("Пользователь не аутентифицирован");
        }

        if (authService.IsAdmin)
        {
            await documentRepository.RemoveHardAsync(id);
        }
        else
        {
            await documentRepository.RemoveSoftAsync(id, authService.IdCurrentUser.Value, authService.IdAdmin, DateTime.Now);
        }
    }

    private static void TrimDocumentProperties(Document document)
    {
        if (document.OutgoingDocumentNumberInputDocument is not null)
        {
            document.OutgoingDocumentNumberInputDocument = document.OutgoingDocumentNumberInputDocument.Trim();
        }

        if (document.CustomerInputDocument is not null)
        {
            document.CustomerInputDocument = document.CustomerInputDocument.Trim();
        }

        if (document.DocumentSummaryInputDocument is not null)
        {
            document.DocumentSummaryInputDocument = document.DocumentSummaryInputDocument.Trim();
        }

        if (document.IncomingDocumentNumberInputDocument is not null)
        {
            document.IncomingDocumentNumberInputDocument = document.IncomingDocumentNumberInputDocument.Trim();
        }

        if (document.ResponsibleDepartmentsInputDocument is not null)
        {
            document.ResponsibleDepartmentsInputDocument = document.ResponsibleDepartmentsInputDocument.Trim();
        }

        if (document.OutgoingDocumentNumberOutputDocument is not null)
        {
            document.OutgoingDocumentNumberOutputDocument = document.OutgoingDocumentNumberOutputDocument.Trim();
        }

        if (document.RecipientOutputDocument is not null)
        {
            document.RecipientOutputDocument = document.RecipientOutputDocument.Trim();
        }

        if (document.DocumentSummaryOutputDocument is not null)
        {
            document.DocumentSummaryOutputDocument = document.DocumentSummaryOutputDocument.Trim();
        }
    }

    public async Task<byte[]> CreateDocumentCsvAsync(int id)
    {                                               
        var documentForCsvExportModel = await documentQuery.GetDocumentForCsvExportAsync(id) ?? 
                                                throw new NotFoundException(nameof(DocumentForCsvExportModel), id);

        var bytesDocument = exportService.ConvertDocumentToCsv(documentForCsvExportModel);

        return bytesDocument;
    }
}