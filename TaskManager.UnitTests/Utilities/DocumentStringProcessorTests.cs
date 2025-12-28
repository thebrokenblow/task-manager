using TaskManager.Application.Utilities;

namespace TaskManager.UnitTests.Utilities;

/// <summary>
/// Набор unit-тестов для класса <see cref="DocumentStringProcessor"/>.
/// </summary>
public class DocumentStringProcessorTests
{
    #region Основные сценарии

    [Fact]
    public void ProcessDocumentSubject_WhenStringContainsId_ShouldRemoveIdAndAddPrefix()
    {
        // Arrange
        var input = "документ id проекта";
        var expected = "id_документ_проекта";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenStringDoesNotContainId_ShouldAddPrefixAndConvertSpaces()
    {
        // Arrange
        var input = "техническое задание проекта";
        var expected = "id_техническое_задание_проекта";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion

    #region Граничные случаи и null-обработка

    [Fact]
    public void ProcessDocumentSubject_WhenInputIsNull_ShouldReturnNull()
    {
        // Arrange
        string? input = null;

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenInputIsEmptyString_ShouldReturnEmptyString()
    {
        // Arrange
        var input = string.Empty;
        var expected = string.Empty;

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenInputIsOnlySpaces_ShouldReturnEmptyString()
    {
        // Arrange
        var input = "     ";
        var expected = string.Empty;

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenStringIsOnlyId_ShouldReturnEmptyString()
    {
        // Arrange
        var input = "ID";
        var expected = string.Empty;

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenStringContainsOnlyIdWithSpaces_ShouldReturnEmptyString()
    {
        // Arrange
        var input = "   ID   ";
        var expected = string.Empty;

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenStringContainsMultipleIdOnly_ShouldReturnEmptyString()
    {
        // Arrange
        var input = "id id ID Id";
        var expected = string.Empty;

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion

    #region Сценарии с пробелами и форматированием

    [Fact]
    public void ProcessDocumentSubject_WhenStringHasMultipleSpaces_ShouldCleanAndAddPrefix()
    {
        // Arrange
        var input = "   проект   id   документация   ";
        var expected = "id_проект_документация";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenStringContainsIdInDifferentCases_ShouldRemoveAllCases()
    {
        // Arrange
        var input = "Документ ID проекта ID id Id";
        var expected = "id_документ_проекта";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion

    #region Сценарии с русским языком

    [Fact]
    public void ProcessDocumentSubject_WhenRussianWordContainsId_ShouldRemoveIdFromWord()
    {
        // Arrange
        var input = "идентификатор документа";
        var expected = "id_идентификатор_документа";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenMixedRussianEnglish_ShouldProcessCorrectly()
    {
        // Arrange
        var input = "Project ID Документация проекта";
        var expected = "id_project_документация_проекта";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion

    #region Реальные примеры документов

    [Fact]
    public void ProcessDocumentSubject_ForTechnicalSpecificationWithId_ShouldFormatCorrectly()
    {
        // Arrange
        var input = "Техническое задание id проекта";
        var expected = "id_техническое_задание_проекта";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_ForContractDocumentWithoutId_ShouldFormatCorrectly()
    {
        // Arrange
        var input = "Договор подряда на выполнение работ";
        var expected = "id_договор_подряда_на_выполнение_работ";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_ForFinancialReportWithId_ShouldFormatCorrectly()
    {
        // Arrange
        var input = "Финансовый отчет ID квартала 2024";
        var expected = "id_финансовый_отчет_квартала_2024";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion

    #region Комплексные сценарии

    [Fact]
    public void ProcessDocumentSubject_WhenComplexDocumentName_ShouldProcessAllComponents()
    {
        // Arrange
        var input = "Финальный отчет ID по проекту разработки ПО за 2024 год";
        var expected = "id_финальный_отчет_по_проекту_разработки_по_за_2024_год";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenIdAtBeginningAndEnd_ShouldRemoveAllAndAddPrefix()
    {
        // Arrange
        var input = "ID документа проекта ID";
        var expected = "id_документа_проекта";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion

    #region Сценарии с частичным содержанием "id"

    [Fact]
    public void ProcessDocumentSubject_WhenStringHasTextAfterIdRemoval_ShouldProcessCorrectly()
    {
        // Arrange
        var input = "ID и еще текст";
        var expected = "id_и_еще_текст";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenStringHasTextBeforeIdRemoval_ShouldProcessCorrectly()
    {
        // Arrange
        var input = "Текст перед ID";
        var expected = "id_текст_перед";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion
}