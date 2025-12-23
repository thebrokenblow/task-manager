using FluentValidation;
using TaskManager.Domain.Model.Documents.Edit;

namespace TaskManager.Application.Validations;

/// <summary>
/// Валидатор для проверки модели изменения статуса документа.
/// </summary>
/// <remarks>
/// Проверяет, что все обязательные поля выходных данных документа заполнены
/// перед изменением статуса на закрытый.
/// </remarks>
public sealed class DocumentForChangeStatusModelValidator : AbstractValidator<DocumentForChangeStatusModel>
{
    /// <summary>
    /// Инициализирует новый экземпляр валидатора для модели изменения статуса документа.
    /// </summary>
    public DocumentForChangeStatusModelValidator()
    {
        RuleFor(model => model)
            .Must(HaveAllRequiredOutputFieldsFilled)
            .WithMessage("Все обязательные поля выходного документа должны быть заполнены");
    }

    /// <summary>
    /// Проверяет, что все обязательные поля выходных данных документа заполнены.
    /// </summary>
    /// <param name="model">Модель документа для изменения статуса.</param>
    /// <returns>
    /// <c>true</c> - если все обязательные поля выходных данных заполнены;
    /// <c>false</c> - если хотя бы одно обязательное поле не заполнено.
    /// </returns>
    /// <remarks>
    /// Обязательные поля выходного документа:
    /// 1. Исходящий номер документа
    /// 2. Получатель документа
    /// 3. Краткое содержание документа
    /// 4. Дата исходящего документа
    /// </remarks>
    private static bool HaveAllRequiredOutputFieldsFilled(DocumentForChangeStatusModel model)
    {
        // Проверяем заполненность строковых полей
        if (string.IsNullOrWhiteSpace(model.OutgoingDocumentNumberOutputDocument) ||
            string.IsNullOrWhiteSpace(model.RecipientOutputDocument) ||
            string.IsNullOrWhiteSpace(model.DocumentSummaryOutputDocument))
        {
            return false;
        }

        // Проверяем заполненность поля даты
        if (!model.OutgoingDocumentDateOutputDocument.HasValue)
        {
            return false;
        }

        return true;
    }
}