using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Model.Documents.Delete;
using TaskManager.Domain.Model.Documents.Edit;
using TaskManager.Domain.Model.Documents.Query;
using TaskManager.Domain.Queries;
using TaskManager.Persistence.Data;
using TaskManager.Persistence.Extensions;

namespace TaskManager.Persistence.Queries;

/// <summary>
/// Предоставляет запросы для работы с данными документов.
/// Реализует сложные сценарии чтения данных с фильтрацией, пагинацией.
/// </summary>
public sealed class DocumentQuery(TaskManagerDbContext context) : IDocumentQuery
{
    private readonly TaskManagerDbContext _context =
        context ?? throw new ArgumentNullException(nameof(context));

    /// <summary>
    /// Получает список документов с фильтрацией, пагинацией и подсчетом общего количества.
    /// </summary>
    /// <param name="documentFilterModel">Модель фильтрации документов.</param>
    /// <param name="countSkip">Количество записей для пропуска (пагинация).</param>
    /// <param name="countTake">Количество записей для получения (пагинация).</param>
    /// <returns>
    /// Задача, результат которой содержит кортеж с двумя значениями:
    /// - Перечисление моделей <see cref="DocumentForOverviewModel"/> с примененными фильтрами
    /// - Общее количество документов, соответствующих фильтрам (до применения пагинации)
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если <paramref name="documentFilterModel"/> равен <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Выбрасывается, если <paramref name="countSkip"/> или <paramref name="countTake"/> имеют недопустимые значения.
    /// </exception>
    /// <remarks>
    /// <para>Логика фильтрации:</para>
    /// <para>1. Если задан поисковый термин или диапазон дат исходящего документа - применяется расширенная фильтрация.</para>
    /// <para>2. Иначе применяется фильтрация по статусам (активные/удаленные/завершенные).</para>
    /// <para>Сортировка: по дате выполнения задачи, затем по флагу контроля.</para>
    /// </remarks>
    public async Task<(IEnumerable<DocumentForOverviewModel> documents, int countDocuments)> GetDocumentsAsync(
        DocumentFilterModel documentFilterModel,
        int countSkip,
        int countTake)
    {
        ArgumentNullException.ThrowIfNull(documentFilterModel);

        if (countSkip < 0)
        {
            throw new ArgumentException("Количество пропускаемых записей не может быть отрицательным", nameof(countSkip));
        }

        if (countTake <= 0)
        {
            throw new ArgumentException("Количество получаемых записей должно быть положительным", nameof(countTake));
        }

        var queryDocuments = _context.Documents
            .Include(document => document.ResponsibleEmployeeInputDocument)
            .AsQueryable();

        queryDocuments = queryDocuments
            .FilterByOutputDate(documentFilterModel)
            .FilterByResponsibleEmployee(documentFilterModel);

        if (!string.IsNullOrWhiteSpace(documentFilterModel.SearchTerm) ||
            documentFilterModel.StartOutgoingDocumentDateOutputDocument.HasValue ||
            documentFilterModel.EndOutgoingDocumentDateOutputDocument.HasValue)
        {
            queryDocuments = queryDocuments
                .FilterByDocumentSearchTerm(documentFilterModel.SearchTerm)
                .FilterByOutputDate(documentFilterModel);
        }
        else
        {
            queryDocuments = queryDocuments.FilterDocumentStatus(documentFilterModel);
        }

        var countDocuments = await queryDocuments.CountAsync();

        var documents = await queryDocuments
            .OrderBy(document => document.TaskDueDateInputDocument)
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
    /// <returns>
    /// Задача, результат которой содержит модель <see cref="DocumentForOverviewEditModel"/> 
    /// или <c>null</c>, если документ не найден.
    /// </returns>
    /// <remarks>
    /// Запрос возвращает все поля документа, необходимые для формы редактирования.
    /// </remarks>
    public async Task<DocumentForOverviewEditModel?> GetDocumentForEditAsync(int id)
    {
        var document = await _context.Documents
            .Select(document => new DocumentForOverviewEditModel
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
                SubjectOutputDocument = document.SubjectOutputDocument
            })
            .AsNoTracking()
            .FirstOrDefaultAsync(document => document.Id == id);

        return document;
    }

    /// <summary>
    /// Получает данные документа для подтверждения удаления.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    /// <returns>
    /// Задача, результат которой содержит модель <see cref="DocumentForOverviewDeleteModel"/> 
    /// или <c>null</c>, если документ не найден.
    /// </returns>
    /// <remarks>
    /// Запрос возвращает основные поля документа, необходимые для отображения перед удалением.
    /// </remarks>
    public async Task<DocumentForOverviewDeleteModel?> GetDocumentForDeleteAsync(int id)
    {
        var document = await _context.Documents
            .Select(document => new DocumentForOverviewDeleteModel
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
    /// Получает идентификатор сотрудника, который удалил документ.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    /// <returns>
    /// Задача, результат которой содержит идентификатор сотрудника, удалившего документ,
    /// или <c>null</c>, если документ не удален или не найден.
    /// </returns>
    public async Task<int?> GetIdEmployeeRemovedAsync(int id)
    {
        var removedByEmployeeId = await _context.Documents
            .Where(document => document.Id == id)
            .Select(document => document.RemovedByEmployeeId)
            .FirstOrDefaultAsync();

        return removedByEmployeeId;
    }

    /// <summary>
    /// Получает идентификатор сотрудника, который создал документ.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    /// <returns>
    /// Задача, результат которой содержит идентификатор сотрудника, создавшего документ,
    /// или <c>null</c>, если документ не найден.
    /// </returns>
    /// <remarks>
    /// Метод проверяет существование документа перед получением идентификатора создателя.
    /// </remarks>
    public async Task<int?> GetIdEmployeeCreatedAsync(int id)
    {
        var isExist = await _context.Documents.AnyAsync(document => document.Id == id);

        if (!isExist)
        {
            return null;
        }

        var createdByEmployeeId = await _context.Documents
            .Where(document => document.Id == id)
            .Select(document => document.CreatedByEmployeeId)
            .FirstOrDefaultAsync();

        return createdByEmployeeId;
    }

    /// <summary>
    /// Получает данные документа для изменения статуса завершения.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    /// <returns>
    /// Задача, результат которой содержит модель <see cref="DocumentForChangeStatusModel"/> 
    /// или <c>null</c>, если документ не найден.
    /// </returns>
    /// <remarks>
    /// Возвращает только поля, которые должны быть заполнены перед закрытием документа.
    /// </remarks>
    public async Task<DocumentForChangeStatusModel?> GetDocumentForChangeStatusAsync(int id)
    {
        var documentForChangeStatusModel = await _context.Documents
            .Where(document => document.Id == id)
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
    /// Получает данные документа для экспорта в CSV-формат.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    /// <returns>
    /// Задача, результат которой содержит модель <see cref="DocumentForOverviewCsvExportModel"/> 
    /// или <c>null</c>, если документ не найден.
    /// </returns>
    /// <remarks>
    /// Возвращает все поля документа, необходимые для формирования CSV-файла.
    /// </remarks>
    public async Task<DocumentForOverviewCsvExportModel?> GetDocumentForCsvExportAsync(int id)
    {
        var documentForExportModel = await _context.Documents
            .Where(document => document.Id == id)
            .Select(document => new DocumentForOverviewCsvExportModel
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