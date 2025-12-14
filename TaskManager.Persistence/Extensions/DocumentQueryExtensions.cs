using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Model.Documents;

namespace TaskManager.Persistence.Extensions;

/// <summary>
/// Методы расширения для запросов к документам.
/// </summary>
public static class DocumentQueryExtensions
{
    /// <summary>
    /// Фильтрует документы по поисковому запросу.
    /// </summary>
    /// <param name="queryDocuments">Запрос документов.</param>
    /// <param name="searchTerm">Поисковый запрос.</param>
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
    /// </summary>
    /// <param name="queryDocuments">Запрос документов.</param>
    /// <param name="documentFilterModel">Модель фильтрации документов.</param>
    public static IQueryable<Document> FilterDocumentStatus(
        this IQueryable<Document> queryDocuments,
        DocumentFilterModel documentFilterModel)
    {
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
    /// <param name="queryDocuments">Запрос документов.</param>
    /// <param name="documentFilterModel">Модель фильтрации документов.</param>
    public static IQueryable<Document> FilterByResponsibleEmployee(
        this IQueryable<Document> queryDocuments,
        DocumentFilterModel documentFilterModel)
    {
        if (documentFilterModel.IdResponsibleEmployeeInputDocument.HasValue)
        {
            queryDocuments = queryDocuments.Where(document =>
                document.IdResponsibleEmployeeInputDocument == documentFilterModel.IdResponsibleEmployeeInputDocument.Value);
        }

        return queryDocuments;
    }

    /// <summary>
    /// Фильтрует документы по дате исходящего документа.
    /// </summary>
    /// <param name="queryDocuments">Запрос документов.</param>
    /// <param name="documentFilterModel">Модель фильтрации документов.</param>
    public static IQueryable<Document> FilterByOutputDate(
        this IQueryable<Document> queryDocuments,
        DocumentFilterModel documentFilterModel)
    {
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