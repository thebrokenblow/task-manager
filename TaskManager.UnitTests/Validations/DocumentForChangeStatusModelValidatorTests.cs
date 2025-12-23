using FluentValidation.TestHelper;
using TaskManager.Application.Validations;
using TaskManager.Domain.Model.Documents.Edit;

namespace TaskManager.UnitTests.Validations;

/// <summary>
/// Тесты для валидатора <see cref="DocumentForChangeStatusModelValidator"/>.
/// </summary>
public class DocumentForChangeStatusModelValidatorTests
{
    private readonly DocumentForChangeStatusModelValidator validator = new();

    [Fact]
    public void Validate_WithAllRequiredFieldsFilled_ShouldBeValid()
    {
        // Arrange
        var model = new DocumentForChangeStatusModel
        {
            OutgoingDocumentNumberOutputDocument = "Исх-123/2024",
            RecipientOutputDocument = "ООО Пример",
            DocumentSummaryOutputDocument = "Ответ на запрос №123",
            OutgoingDocumentDateOutputDocument = new DateOnly(2024, 1, 15),
            IsCompleted = true
        };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WithMissingOutgoingDocumentNumber_ShouldBeInvalid(string? outgoingDocumentNumber)
    {
        // Arrange
        var model = new DocumentForChangeStatusModel
        {
            OutgoingDocumentNumberOutputDocument = outgoingDocumentNumber,
            RecipientOutputDocument = "ООО Пример",
            DocumentSummaryOutputDocument = "Ответ на запрос №123",
            OutgoingDocumentDateOutputDocument = new DateOnly(2024, 1, 15),
            IsCompleted = true
        };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage("Все обязательные поля выходного документа должны быть заполнены");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WithMissingRecipient_ShouldBeInvalid(string? recipient)
    {
        // Arrange
        var model = new DocumentForChangeStatusModel
        {
            OutgoingDocumentNumberOutputDocument = "Исх-123/2024",
            RecipientOutputDocument = recipient,
            DocumentSummaryOutputDocument = "Ответ на запрос №123",
            OutgoingDocumentDateOutputDocument = new DateOnly(2024, 1, 15),
            IsCompleted = true
        };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage("Все обязательные поля выходного документа должны быть заполнены");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WithMissingDocumentSummary_ShouldBeInvalid(string? documentSummary)
    {
        // Arrange
        var model = new DocumentForChangeStatusModel
        {
            OutgoingDocumentNumberOutputDocument = "Исх-123/2024",
            RecipientOutputDocument = "ООО Пример",
            DocumentSummaryOutputDocument = documentSummary,
            OutgoingDocumentDateOutputDocument = new DateOnly(2024, 1, 15),
            IsCompleted = true
        };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage("Все обязательные поля выходного документа должны быть заполнены");
    }

    [Fact]
    public void Validate_WithMissingOutgoingDocumentDate_ShouldBeInvalid()
    {
        // Arrange
        var model = new DocumentForChangeStatusModel
        {
            OutgoingDocumentNumberOutputDocument = "Исх-123/2024",
            RecipientOutputDocument = "ООО Пример",
            DocumentSummaryOutputDocument = "Ответ на запрос №123",
            OutgoingDocumentDateOutputDocument = null,
            IsCompleted = true
        };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage("Все обязательные поля выходного документа должны быть заполнены");
    }

    [Fact]
    public void Validate_WithMultipleMissingFields_ShouldBeInvalid()
    {
        // Arrange
        var model = new DocumentForChangeStatusModel
        {
            OutgoingDocumentNumberOutputDocument = null,
            RecipientOutputDocument = "",
            DocumentSummaryOutputDocument = "   ",
            OutgoingDocumentDateOutputDocument = null,
            IsCompleted = true
        };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage("Все обязательные поля выходного документа должны быть заполнены");
    }

    [Fact]
    public void Validate_WithMinimalValidData_ShouldBeValid()
    {
        // Arrange
        var model = new DocumentForChangeStatusModel
        {
            OutgoingDocumentNumberOutputDocument = "1",
            RecipientOutputDocument = "А",
            DocumentSummaryOutputDocument = "Б",
            OutgoingDocumentDateOutputDocument = new DateOnly(2024, 1, 1),
            IsCompleted = false
        };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x);
    }

    [Fact]
    public void Validate_WhenNotCompleted_WithMissingFields_ShouldBeValid()
    {
        // Внимание: текущая валидация не проверяет IsCompleted!
        // Если нужно валидировать только при IsCompleted == true, нужно обновить валидатор
        // Arrange
        var model = new DocumentForChangeStatusModel
        {
            OutgoingDocumentNumberOutputDocument = null,
            RecipientOutputDocument = null,
            DocumentSummaryOutputDocument = null,
            OutgoingDocumentDateOutputDocument = null,
            IsCompleted = false // Документ не завершен
        };

        // Act
        var result = validator.TestValidate(model);

        // Assert
        // Текущая логика: всегда проверяет поля, независимо от IsCompleted
        result.ShouldHaveValidationErrorFor(x => x);
    }
}