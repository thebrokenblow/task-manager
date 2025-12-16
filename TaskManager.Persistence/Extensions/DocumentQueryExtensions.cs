using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Model.Documents;

namespace TaskManager.Persistence.Extensions;

/// <summary>
/// Методы расширения для фильтрации запросов к документам.
/// Предоставляет набор методов для применения различных фильтров к документам.
/// </summary>
public static class DocumentQueryExtensions
{
    /// <summary>
    /// Фильтрует документы по поисковому запросу.
    /// Поиск выполняется по нескольким текстовым полям документа с регистронезависимым сравнением.
    /// </summary>
    /// <param name="queryDocuments">Запрос документов для фильтрации.</param>
    /// <param name="searchTerm">Поисковый запрос. Если null или пустая строка - фильтр не применяется.</param>
    /// <returns>
    /// Отфильтрованный запрос документов, содержащих указанный поисковый запрос в одном из полей.
    /// </returns>
    public static IQueryable<Document> FilterByDocumentSearchTerm(
        this IQueryable<Document> queryDocuments,
        string? searchTerm)
    {
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            queryDocuments = queryDocuments.Where(document =>
                (document.OutgoingDocumentNumberInputDocument != null &&
                    EF.Functions.ILike(document.OutgoingDocumentNumberInputDocument, $"%{searchTerm}%")) ||

                (document.DocumentSummaryInputDocument != null &&
                    EF.Functions.ILike(document.DocumentSummaryInputDocument, $"%{searchTerm}%")) ||

                (document.IncomingDocumentNumberInputDocument != null &&
                    EF.Functions.ILike(document.IncomingDocumentNumberInputDocument, $"%{searchTerm}%")) ||

                (document.OutgoingDocumentNumberOutputDocument != null &&
                    EF.Functions.ILike(document.OutgoingDocumentNumberOutputDocument, $"%{searchTerm}%")));
        }

        return queryDocuments;
    }

    /// <summary>
    /// Фильтрует документы по статусу в зависимости от роли пользователя.
    /// Применяет различные условия фильтрации для администраторов, руководителей и сотрудников.
    /// </summary>
    /// <param name="queryDocuments">Запрос документов для фильтрации.</param>
    /// <param name="documentFilterModel">Модель фильтрации документов, содержащая роль пользователя.</param>
    /// <returns>
    /// Отфильтрованный запрос документов с учетом роли пользователя и статусов документов.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если <paramref name="documentFilterModel"/> равен <c>null</c>.
    /// </exception>
    public static IQueryable<Document> FilterDocumentStatus(
        this IQueryable<Document> queryDocuments,
        DocumentFilterModel documentFilterModel)
    {
        ArgumentNullException.ThrowIfNull(documentFilterModel);

        if (documentFilterModel.UserRole == UserRole.Admin)
        {
            queryDocuments = queryDocuments.Where(document =>
                document.RemovedByEmployeeId != null &&
                document.RemoveDateTime != null);
        }
        else if (documentFilterModel.UserRole == UserRole.Boss)
        {
            queryDocuments = queryDocuments.Where(document =>
                !document.IsCompleted &&
                document.RemovedByEmployeeId == null &&
                document.RemoveDateTime == null);
        }
        else if (documentFilterModel.UserRole == UserRole.Employee)
        {
            queryDocuments = queryDocuments.Where(document =>
                !document.IsCompleted &&
                document.ResponsibleDepartmentInputDocument == documentFilterModel.ResponsibleDepartmentInputDocument &&
                document.RemovedByEmployeeId == null &&
                document.RemoveDateTime == null);
        }
        else
        {
            queryDocuments = queryDocuments.Where(document =>
                !document.IsCompleted &&
                document.RemovedByEmployeeId == null &&
                document.RemoveDateTime == null);
        }

        return queryDocuments;
    }

    /// <summary>
    /// Фильтрует документы по ответственному сотруднику.
    /// </summary>
    /// <param name="queryDocuments">Запрос документов для фильтрации.</param>
    /// <param name="documentFilterModel">Модель фильтрации документов, содержащая идентификатор ответственного сотрудника.</param>
    /// <returns>
    /// Отфильтрованный запрос документов, закрепленных за указанным сотрудником.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если <paramref name="documentFilterModel"/> равен <c>null</c>.
    /// </exception>
    public static IQueryable<Document> FilterByResponsibleEmployee(
        this IQueryable<Document> queryDocuments,
        DocumentFilterModel documentFilterModel)
    {
        ArgumentNullException.ThrowIfNull(documentFilterModel);

        if (documentFilterModel.IdResponsibleEmployeeInputDocument.HasValue)
        {
            queryDocuments = queryDocuments.Where(document =>
                document.IdResponsibleEmployeeInputDocument == documentFilterModel.IdResponsibleEmployeeInputDocument.Value);
        }

        return queryDocuments;
    }

    /// <summary>
    /// Фильтрует документы по диапазону дат исходящего документа.
    /// </summary>
    /// <param name="queryDocuments">Запрос документов для фильтрации.</param>
    /// <param name="documentFilterModel">Модель фильтрации документов, содержащая диапазон дат.</param>
    /// <returns>
    /// Отфильтрованный запрос документов, у которых дата исходящего документа находится в указанном диапазоне.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если <paramref name="documentFilterModel"/> равен <c>null</c>.
    /// </exception>
    public static IQueryable<Document> FilterByOutputDate(
        this IQueryable<Document> queryDocuments,
        DocumentFilterModel documentFilterModel)
    {
        ArgumentNullException.ThrowIfNull(documentFilterModel);

        if (documentFilterModel.StartOutgoingDocumentDateOutputDocument.HasValue &&
            documentFilterModel.EndOutgoingDocumentDateOutputDocument.HasValue)
        {
            queryDocuments = queryDocuments.Where(document =>
                document.OutgoingDocumentDateOutputDocument.HasValue &&
                document.OutgoingDocumentDateOutputDocument.Value.Year >= documentFilterModel.StartOutgoingDocumentDateOutputDocument.Value.Year &&
                document.OutgoingDocumentDateOutputDocument.Value.Month >= documentFilterModel.StartOutgoingDocumentDateOutputDocument.Value.Month &&
                document.OutgoingDocumentDateOutputDocument.Value.Day >= documentFilterModel.StartOutgoingDocumentDateOutputDocument.Value.Day &&

                document.OutgoingDocumentDateOutputDocument.Value.Year <= documentFilterModel.EndOutgoingDocumentDateOutputDocument.Value.Year &&
                document.OutgoingDocumentDateOutputDocument.Value.Month <= documentFilterModel.EndOutgoingDocumentDateOutputDocument.Value.Month &&
                document.OutgoingDocumentDateOutputDocument.Value.Day <= documentFilterModel.EndOutgoingDocumentDateOutputDocument.Value.Day);
        }

        return queryDocuments;
    }
}