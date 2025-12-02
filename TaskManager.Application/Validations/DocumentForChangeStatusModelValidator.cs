using FluentValidation;
using TaskManager.Domain.Model.Documents;

namespace TaskManager.Application.Validations;


public class DocumentForChangeStatusModelValidator : AbstractValidator<DocumentForChangeStatusModel>
{
    public DocumentForChangeStatusModelValidator()
    {
        RuleFor(documentForChangeStatusModel => documentForChangeStatusModel)
            .Must(HaveAllRequiredFieldsFilled)
            .WithMessage("Все поля выходного документа должны быть заполнены");
    }

    private static bool HaveAllRequiredFieldsFilled(DocumentForChangeStatusModel documentForChangeStatusModel)
    {
        if (string.IsNullOrWhiteSpace(documentForChangeStatusModel.OutgoingDocumentNumberOutputDocument) ||
            string.IsNullOrWhiteSpace(documentForChangeStatusModel.RecipientOutputDocument) ||
            string.IsNullOrWhiteSpace(documentForChangeStatusModel.DocumentSummaryOutputDocument))
        {
            return false;
        }

        if (!documentForChangeStatusModel.OutgoingDocumentDateOutputDocument.HasValue)
        {
            return false;
        }

        return true;
    }
}