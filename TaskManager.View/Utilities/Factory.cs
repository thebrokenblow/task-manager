using TaskManager.Application.Dtos.Documents;

namespace TaskManager.View.Utilities;

/// <summary>
/// Фабрика для создания объектов с предустановленными значениями по умолчанию.
/// Предоставляет методы для создания DTO и других объектов с инициализированными свойствами.
/// </summary>
public class Factory
{
    /// <summary>
    /// Смещение дней по умолчанию для установки срока выполнения задачи.
    /// Определяет, через сколько дней от текущей даты устанавливается срок выполнения по умолчанию.
    /// </summary>
    private const int DefaultDueDateDaysOffset = 5;

    /// <summary>
    /// Создает DTO документа с предустановленными значениями по умолчанию.
    /// Инициализирует обязательные поля значениями по умолчанию и устанавливает необязательные поля в null.
    /// </summary>
    /// <returns>Экземпляр CreatedDocumentDto с заполненными значениями по умолчанию.</returns>
    public static CreatedDocumentDto CreateDefaultDocument()
    {
        var document = new CreatedDocumentDto
        {
            IsExternalDocumentInputDocument = true,
            IncomingDocumentNumberInputDocument = string.Empty,
            IncomingDocumentDateInputDocument = DateOnly.FromDateTime(DateTime.Today),
            TaskDueDateInputDocument = DateOnly.FromDateTime(DateTime.Today.AddDays(DefaultDueDateDaysOffset)),
            IsExternalDocumentOutputDocument = true,
            IsUnderControl = false,
            OutgoingDocumentDateOutputDocument = null,
            OutgoingDocumentNumberInputDocument = null,
            CustomerInputDocument = null,
            DocumentSummaryInputDocument = null,
            DocumentSummaryOutputDocument = null,
            IdResponsibleEmployeeInputDocument = null,
            OutgoingDocumentNumberOutputDocument = null,
            RecipientOutputDocument = null,
            ResponsibleDepartmentInputDocument = null,
            ResponsibleDepartmentsInputDocument = null,
            SourceDocumentDateInputDocument = null,
            SubjectOutputDocument = null,
        };

        return document;
    }
}