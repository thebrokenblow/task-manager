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

/// <summary>
/// Реализация сервиса для управления документами.
/// </summary>
/// <remarks>
/// Обеспечивает бизнес-логику работы с документами, включая управление доступом,
/// валидацию данных и взаимодействие с репозиториями.
/// </remarks>
public class DocumentService(
    IDocumentQuery documentQuery,
    IDocumentRepository documentRepository,
    IAuthService authService,
    IExportService exportService) : IDocumentService
{
    /// <inheritdoc/>
    public async Task<PagedResult<DocumentForOverviewModel>> GetPagedAsync(
        string? searchTerm,
        bool showMyTasks,
        DateOnly? startOutgoingDocumentDateOutputDocument,
        DateOnly? endOutgoingDocumentDateOutputDocument,
        int page, 
        int pageSize)
    {
        if (showMyTasks && !authService.IsAuthenticated)
        {
            throw new UnauthorizedAccessException();
        }

        int? idResponsibleEmployeeInputDocument = null;

        if (showMyTasks)
        {
            idResponsibleEmployeeInputDocument = authService.IdCurrentUser;
        }

        int countDocuments;
        List<DocumentForOverviewModel> documents;

        int countSkip = (page - 1) * pageSize;

        if (authService.IsAdmin)
        {
            // Администраторы видят все документы, включая архивные
            (documents, countDocuments) = await documentQuery.GetDocumentsAsync(
                searchTerm,
                startOutgoingDocumentDateOutputDocument,
                endOutgoingDocumentDateOutputDocument,
                idResponsibleEmployeeInputDocument,
                countSkip,
                pageSize,
                DocumentStatus.Archived);
        }
        else
        {
            // Обычные пользователи видят только активные документы
            (documents, countDocuments) = await documentQuery.GetDocumentsAsync(
                searchTerm,
                startOutgoingDocumentDateOutputDocument,
                endOutgoingDocumentDateOutputDocument,
                idResponsibleEmployeeInputDocument,
                countSkip,
                pageSize,
                DocumentStatus.Active);
        }

        var pagedResult = new PagedResult<DocumentForOverviewModel>(
            documents,
            countDocuments,
            page,
            pageSize);

        return pagedResult;
    }

    /// <inheritdoc/>
    public async Task<DocumentForEditModel?> GetDocumentForEditAsync(int id)
    {
        var document = await documentQuery.GetDocumentForEditAsync(id);
        return document;
    }

    /// <inheritdoc/>
    public async Task<DocumentForDeleteModel?> GetDocumentForDeleteAsync(int id)
    {
        var document = await documentQuery.GetDocumentForDeleteAsync(id);
        return document;
    }

    /// <inheritdoc/>
    public async Task<Document?> GetByIdAsync(int id)
    {
        var document = await documentRepository.GetByIdAsync(id);
        return document;
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public async Task CreateAsync(Document document)
    {
        if (!authService.IsAuthenticated || !authService.IdCurrentUser.HasValue)
        {
            throw new UnauthorizedAccessException("Пользователь не аутентифицирован");
        }

        TrimDocumentStrings(document);

        document.CreatedByEmployeeId = authService.IdCurrentUser.Value;
        await documentRepository.AddAsync(document);
    }

    /// <inheritdoc/>
    public async Task EditAsync(Document document)
    {
        TrimDocumentStrings(document);

        document.LastEditedDateTime = DateTime.Now;
        document.LastEditedByEmployeeId = authService.IdCurrentUser;

        await documentRepository.UpdateAsync(document);
    }

    /// <inheritdoc/>
    public async Task RecoverDeletedAsync(int id)
    {
        var removedByEmployeeId = await documentQuery.GetIdEmployeeRemovedAsync(id) ??
            throw new InvalidOperationException(
                $"Не удалось определить сотрудника, удалившего документ с ID: {id}");

        await documentRepository.RecoverDeletedAsync(id, removedByEmployeeId);
    }

    /// <inheritdoc/>
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
            await documentRepository.RemoveSoftAsync(
                id,
                authService.IdCurrentUser.Value,
                authService.IdAdmin,
                DateTime.Now);
        }
    }

    /// <inheritdoc/>
    public async Task<byte[]> CreateDocumentCsvAsync(int id)
    {
        var documentForCsvExportModel = await documentQuery.GetDocumentForCsvExportAsync(id) ??
            throw new NotFoundException(nameof(DocumentForCsvExportModel), id);

        var bytesDocument = exportService.ConvertDocumentToCsv(documentForCsvExportModel);
        return bytesDocument;
    }

    /// <summary>
    /// Очищает строковые свойства документа от лишних пробелов.
    /// </summary>
    /// <param name="document">Документ для обработки.</param>
    private static void TrimDocumentStrings(Document document)
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
}