using TaskManager.Application.Dtos.Documents;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Model.Documents.Edit;

namespace TaskManager.Application.Factories;

public static class DocumentFactory
{
    /// <summary>
    /// Создает новый экземпляр документа из DTO.
    /// </summary>
    /// <param name="createdDocumentDto">DTO с данными документа.</param>
    /// <param name="currentUserId">Идентификатор текущего пользователя.</param>
    /// <returns>Новый экземпляр документа.</returns>
    public static Document CreateDocument(CreatedDocumentDto createdDocumentDto, int currentUserId)
    {
        ArgumentNullException.ThrowIfNull(createdDocumentDto);

        var document = new Document
        {
            OutgoingDocumentNumberInputDocument = createdDocumentDto.OutgoingDocumentNumberInputDocument,
            SourceDocumentDateInputDocument = createdDocumentDto.SourceDocumentDateInputDocument,
            CustomerInputDocument = createdDocumentDto.CustomerInputDocument,
            DocumentSummaryInputDocument = createdDocumentDto.DocumentSummaryInputDocument,
            IsExternalDocumentInputDocument = createdDocumentDto.IsExternalDocumentInputDocument,
            IncomingDocumentNumberInputDocument = createdDocumentDto.IncomingDocumentNumberInputDocument,
            IncomingDocumentDateInputDocument = createdDocumentDto.IncomingDocumentDateInputDocument,
            ResponsibleDepartmentInputDocument = createdDocumentDto.ResponsibleDepartmentInputDocument,
            ResponsibleDepartmentsInputDocument = createdDocumentDto.ResponsibleDepartmentsInputDocument,
            TaskDueDateInputDocument = createdDocumentDto.TaskDueDateInputDocument,
            IdResponsibleEmployeeInputDocument = createdDocumentDto.IdResponsibleEmployeeInputDocument,
            IsExternalDocumentOutputDocument = createdDocumentDto.IsExternalDocumentOutputDocument,
            OutgoingDocumentNumberOutputDocument = createdDocumentDto.OutgoingDocumentNumberOutputDocument,
            OutgoingDocumentDateOutputDocument = createdDocumentDto.OutgoingDocumentDateOutputDocument,
            RecipientOutputDocument = createdDocumentDto.RecipientOutputDocument,
            DocumentSummaryOutputDocument = createdDocumentDto.DocumentSummaryOutputDocument,
            IsUnderControl = createdDocumentDto.IsUnderControl,
            IsCompleted = false,
            CreatedByEmployeeId = currentUserId,
            SubjectOutputDocument = createdDocumentDto.SubjectOutputDocument
        };

        return document;
    }

    /// <summary>
    /// Создает модель для редактирования документа из DTO.
    /// </summary>
    /// <param name="editedDocumentDto">DTO с данными документа для редактирования.</param>
    /// <param name="currentUserId">Идентификатор текущего пользователя.</param>
    /// <returns>Модель для редактирования документа.</returns>
    public static DocumentForEditModel CreateDocumentForEdit(EditedDocumentDto editedDocumentDto, int currentUserId)
    {
        ArgumentNullException.ThrowIfNull(editedDocumentDto);

        var documentForEditModel = new DocumentForEditModel
        {
            Id = editedDocumentDto.Id,
            OutgoingDocumentNumberInputDocument = editedDocumentDto.OutgoingDocumentNumberInputDocument,
            SourceDocumentDateInputDocument = editedDocumentDto.SourceDocumentDateInputDocument,
            CustomerInputDocument = editedDocumentDto.CustomerInputDocument,
            DocumentSummaryInputDocument = editedDocumentDto.DocumentSummaryInputDocument,
            IsExternalDocumentInputDocument = editedDocumentDto.IsExternalDocumentInputDocument,
            IncomingDocumentNumberInputDocument = editedDocumentDto.IncomingDocumentNumberInputDocument,
            IncomingDocumentDateInputDocument = editedDocumentDto.IncomingDocumentDateInputDocument,
            ResponsibleDepartmentInputDocument = editedDocumentDto.ResponsibleDepartmentInputDocument,
            ResponsibleDepartmentsInputDocument = editedDocumentDto.ResponsibleDepartmentsInputDocument,
            TaskDueDateInputDocument = editedDocumentDto.TaskDueDateInputDocument,
            IdResponsibleEmployeeInputDocument = editedDocumentDto.IdResponsibleEmployeeInputDocument,
            IsExternalDocumentOutputDocument = editedDocumentDto.IsExternalDocumentOutputDocument,
            OutgoingDocumentNumberOutputDocument = editedDocumentDto.OutgoingDocumentNumberOutputDocument,
            OutgoingDocumentDateOutputDocument = editedDocumentDto.OutgoingDocumentDateOutputDocument,
            RecipientOutputDocument = editedDocumentDto.RecipientOutputDocument,
            DocumentSummaryOutputDocument = editedDocumentDto.DocumentSummaryOutputDocument,
            IsUnderControl = editedDocumentDto.IsUnderControl,
            IsCompleted = editedDocumentDto.IsCompleted,
            LastEditedDateTime = DateTime.Now,
            LastEditedByEmployeeId = currentUserId,
            SubjectOutputDocument = editedDocumentDto.SubjectOutputDocument,
        };

        return documentForEditModel;
    }
}