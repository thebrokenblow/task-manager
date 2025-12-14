using TaskManager.Application.Common;
using TaskManager.Application.Dtos.Documents;
using TaskManager.Application.Exceptions;
using TaskManager.Application.Services.Interfaces;
using TaskManager.Application.Validations;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Exceptions;
using TaskManager.Domain.Model.Departments;
using TaskManager.Domain.Model.Documents;
using TaskManager.Domain.Queries;
using TaskManager.Domain.Repositories;
using TaskManager.Domain.Services;

namespace TaskManager.Application.Services;

/// <summary>
/// Сервис для управления документами.
/// </summary>
public class DocumentService(
    IDepartmentQuery departmentQuery,
    IDocumentQuery documentQuery,
    IDocumentRepository documentRepository,
    IAuthService authService,
    IExportService exportService) : IDocumentService
{
    /// <summary>
    /// Получает постраничный список документов.
    /// </summary>
    /// <param name="documentFilterDto">Модель фильтрации документов.</param>
    /// <param name="page">Номер страницы.</param>
    /// <param name="pageSize">Размер страницы.</param>
    public async Task<PagedResult<DocumentForOverviewModel>> GetPagedAsync(
        DocumentFilterDto documentFilterDto,
        int page,
        int pageSize)
    {
        int? idResponsibleEmployeeInputDocument = null;

        if (documentFilterDto.IsShowMyTasks)
        {
            idResponsibleEmployeeInputDocument = authService.IdCurrentUser;
        }

        int countDocuments;
        List<DocumentForOverviewModel> documents;

        int countSkip = (page - 1) * pageSize;

        DepartmentModel? departmentModel = null;
        if (authService.IsAuthenticated && authService.IdCurrentUser.HasValue)
        {
            departmentModel = await departmentQuery.GetDepartmentByEmployeeIdAsync(authService.IdCurrentUser.Value);
        }

        var documentFilterModel = new DocumentFilterModel
        {
            SearchTerm = documentFilterDto.SearchTerm,
            StartOutgoingDocumentDateOutputDocument = documentFilterDto.StartOutgoingDocumentDateOutputDocument,
            EndOutgoingDocumentDateOutputDocument = documentFilterDto.EndOutgoingDocumentDateOutputDocument,
            IdResponsibleEmployeeInputDocument = idResponsibleEmployeeInputDocument,
            ResponsibleDepartmentInputDocument = departmentModel?.Name,
            UserRole = authService.Role,
        };

        (documents, countDocuments) = await documentQuery.GetDocumentsAsync(
            documentFilterModel,
            countSkip,
            pageSize);

        var pagedResult = new PagedResult<DocumentForOverviewModel>(
            documents,
            countDocuments,
            page,
            pageSize);

        return pagedResult;
    }

    /// <summary>
    /// Получает данные документа для редактирования.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    public async Task<DocumentForEditModel?> GetDocumentForEditAsync(int id)
    {
        var document = await documentQuery.GetDocumentForEditAsync(id);
        return document;
    }

    /// <summary>
    /// Получает данные документа для удаления.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    public async Task<DocumentForDeleteModel?> GetDocumentForDeleteAsync(int id)
    {
        var document = await documentQuery.GetDocumentForDeleteAsync(id);
        return document;
    }

    /// <summary>
    /// Получает документ по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    public async Task<Document?> GetByIdAsync(int id)
    {
        var document = await documentRepository.GetByIdAsync(id);
        return document;
    }

    /// <summary>
    /// Изменяет статус завершения документа.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
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

    /// <summary>
    /// Создает новый документ.
    /// </summary>
    /// <param name="document">Документ для создания.</param>
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

    /// <summary>
    /// Редактирует существующий документ.
    /// </summary>
    /// <param name="document">Документ с обновленными данными.</param>
    public async Task EditAsync(Document document)
    {
        TrimDocumentStrings(document);

        document.LastEditedDateTime = DateTime.Now;
        document.LastEditedByEmployeeId = authService.IdCurrentUser;

        await documentRepository.UpdateAsync(document);
    }

    /// <summary>
    /// Восстанавливает удаленный документ.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    public async Task RecoverDeletedAsync(int id)
    {
        var removedByEmployeeId = await documentQuery.GetIdEmployeeRemovedAsync(id) ??
            throw new InvalidOperationException(
                $"Не удалось определить сотрудника, удалившего документ с ID: {id}");

        await documentRepository.RecoverDeletedAsync(id, removedByEmployeeId);
    }

    /// <summary>
    /// Удаляет документ.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
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

    /// <summary>
    /// Создает CSV-файл с данными документа.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
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