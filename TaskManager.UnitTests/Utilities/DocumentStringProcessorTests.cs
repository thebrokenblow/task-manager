using TaskManager.Application.Utilities;

namespace TaskManager.UnitTests.Utilities;

/// <summary>
/// Набор unit-тестов для класса <see cref="DocumentStringProcessor"/>.
/// </summary>
public class DocumentStringProcessorTests
{
    #region Основные сценарии

    [Fact]
    public void ProcessDocumentSubject_WhenStringContainsId_ReturnsStringWithoutIdAndWithPrefix()
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
    public void ProcessDocumentSubject_WhenStringDoesNotContainId_ReturnsStringWithPrefixAndUnderscores()
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
    public void ProcessDocumentSubject_WhenInputIsNull_ReturnsNull()
    {
        // Arrange
        string? input = null;

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenInputIsEmptyString_ReturnsEmptyString()
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
    public void ProcessDocumentSubject_WhenInputIsWhitespaceOnly_ReturnsEmptyString()
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
    public void ProcessDocumentSubject_WhenStringIsOnlyId_ReturnsEmptyString()
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
    public void ProcessDocumentSubject_WhenStringIsOnlyIdWithSurroundingSpaces_ReturnsEmptyString()
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
    public void ProcessDocumentSubject_WhenStringContainsMultipleIdsOnly_ReturnsEmptyString()
    {
        // Arrange
        var input = "id id ID Id";
        var expected = string.Empty;

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenStringIsOnlyIdWithSeparators_ReturnsEmptyString()
    {
        // Arrange
        var input = "ID_ID_ID";
        var expected = string.Empty;

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion

    #region Граничные случаи с разными формами "id"

    [Fact]
    public void ProcessDocumentSubject_WhenInputIsOnlyLowercaseId_ReturnsEmptyString()
    {
        // Arrange
        var input = "id";
        var expected = string.Empty;

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenInputIsOnlyIdWithUnderscore_ReturnsEmptyString()
    {
        // Arrange
        var input = "id_";
        var expected = string.Empty;

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenInputStartsWithUnderscoreAndId_ReturnsEmptyString()
    {
        // Arrange
        var input = "_id";
        var expected = string.Empty;

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenInputHasIdSurroundedByUnderscores_ReturnsEmptyString()
    {
        // Arrange
        var input = "_id_";
        var expected = string.Empty;

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenInputHasMultipleIdsWithUnderscores_ReturnsEmptyString()
    {
        // Arrange
        var input = "id_id_id";
        var expected = string.Empty;

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenInputHasIdWithUnderscoreAndText_ReturnsTextWithPrefix()
    {
        // Arrange
        var input = "id_документ";
        var expected = "id_документ";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenInputHasTextAndIdWithUnderscore_ReturnsTextWithPrefix()
    {
        // Arrange
        var input = "документ_id";
        var expected = "id_документ";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenInputHasTextIdTextWithUnderscores_ReturnsTextsWithPrefix()
    {
        // Arrange
        var input = "текст1_id_текст2";
        var expected = "id_текст1_текст2";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion

    #region Сценарии с пробелами и форматированием

    [Fact]
    public void ProcessDocumentSubject_WhenStringHasMultipleSpaces_ReturnsCleanedStringWithPrefix()
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
    public void ProcessDocumentSubject_WhenStringHasTabsAndNewLines_ReturnsCleanedString()
    {
        // Arrange
        var input = "проект\tid\nдокументация";
        var expected = "id_проект_документация";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenStringContainsIdInDifferentCases_RemovesAllCaseVariations()
    {
        // Arrange
        var input = "Документ ID проекта ID id Id iD";
        var expected = "id_документ_проекта";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion

    #region Сценарии с табуляцией и переносами строк

    [Fact]
    public void ProcessDocumentSubject_WhenStringContainsTabs_ReturnsCleanedString()
    {
        // Arrange
        var input = "проект\tid\tдокументация";
        var expected = "id_проект_документация";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenStringContainsNewLines_ReturnsCleanedString()
    {
        // Arrange
        var input = "проект\nid\nдокументация";
        var expected = "id_проект_документация";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenStringContainsCarriageReturnAndNewLine_ReturnsCleanedString()
    {
        // Arrange
        var input = "проект\r\nid\r\nдокументация";
        var expected = "id_проект_документация";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenStringWithMixedWhitespaceCharacters_ReturnsCleanedString()
    {
        // Arrange
        var input = "  проект \t\n id  \r\n документация  ";
        var expected = "id_проект_документация";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenStringWithMultipleTabs_ReturnsCleanedString()
    {
        // Arrange
        var input = "\t\tпроект\t\tid\t\tдокументация\t\t";
        var expected = "id_проект_документация";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenStringWithMultipleNewLines_ReturnsCleanedString()
    {
        // Arrange
        var input = "\n\nпроект\n\nid\n\nдокументация\n\n";
        var expected = "id_проект_документация";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenStringWithTabsAndId_RemovesIdAndProcessesCorrectly()
    {
        // Arrange
        var input = "\tID\tпроекта\t";
        var expected = "id_проекта";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenStringWithNewLinesAndId_RemovesIdAndProcessesCorrectly()
    {
        // Arrange
        var input = "\nID\nпроекта\n";
        var expected = "id_проекта";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenStringWithWindowsLineEndings_ReturnsCleanedString()
    {
        // Arrange
        var input = "Проект\r\nID\r\nДокументация";
        var expected = "id_проект_документация";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenStringWithUnixLineEndings_ReturnsCleanedString()
    {
        // Arrange
        var input = "Проект\nID\nДокументация";
        var expected = "id_проект_документация";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenStringWithOnlyTabs_ReturnsEmptyString()
    {
        // Arrange
        var input = "\t\t\t";
        var expected = string.Empty;

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenStringWithOnlyNewLines_ReturnsEmptyString()
    {
        // Arrange
        var input = "\n\n\n";
        var expected = string.Empty;

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenStringWithMixedWhitespaceAndIdOnly_ReturnsEmptyString()
    {
        // Arrange
        var input = " \t\nID\t\n ";
        var expected = string.Empty;

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenStringWithTabsInMiddleOfWords_ProcessesCorrectly()
    {
        // Arrange
        var input = "про\tект до\tку\tментация";
        var expected = "id_про_ект_до_ку_ментация";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenStringWithNewLinesInMiddleOfWords_ProcessesCorrectly()
    {
        // Arrange
        var input = "про\nект до\nку\nментация";
        var expected = "id_про_ект_до_ку_ментация";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion

    #region Сценарии с различными языками

    [Fact]
    public void ProcessDocumentSubject_WhenRussianWordContainsIdPart_ReturnsWordWithoutRemovingIdPart()
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
    public void ProcessDocumentSubject_WhenMixedRussianEnglishText_ProcessesCorrectly()
    {
        // Arrange
        var input = "Project ID Документация проекта";
        var expected = "id_project_документация_проекта";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenStringContainsCyrillicId_ProcessesCorrectly()
    {
        // Arrange
        var input = "Документ айди проекта";
        var expected = "id_документ_айди_проекта";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion

    #region Реальные примеры документов

    [Fact]
    public void ProcessDocumentSubject_ForTechnicalSpecification_ReturnsFormattedString()
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
    public void ProcessDocumentSubject_ForContractDocument_ReturnsFormattedString()
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
    public void ProcessDocumentSubject_ForFinancialReport_ReturnsFormattedString()
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
    public void ProcessDocumentSubject_WhenComplexDocumentName_ProcessesAllComponents()
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
    public void ProcessDocumentSubject_WhenIdAtBeginningAndEnd_RemovesAllIds()
    {
        // Arrange
        var input = "ID документа проекта ID";
        var expected = "id_документа_проекта";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenMultipleIdsInMiddle_RemovesAllIds()
    {
        // Arrange
        var input = "Отчет ID проекта ID разработки";
        var expected = "id_отчет_проекта_разработки";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion

    #region Сценарии с позиционированием "id"

    [Fact]
    public void ProcessDocumentSubject_WhenIdAtBeginningWithTextAfter_ReturnsTextWithPrefix()
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
    public void ProcessDocumentSubject_WhenIdInMiddle_ReturnsTextWithIdRemoved()
    {
        // Arrange
        var input = "Текст перед ID текст после";
        var expected = "id_текст_перед_текст_после";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenIdAtEnd_ReturnsTextWithoutId()
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

    #region Сценарии с подчеркиваниями

    [Fact]
    public void ProcessDocumentSubject_WhenStringWithUnderscores_ReturnsFormattedString()
    {
        // Arrange
        var input = "ID_и_еще_текст";
        var expected = "id_и_еще_текст";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenMixedSeparators_ReturnsStringWithUnderscores()
    {
        // Arrange
        var input = "Project_ID документация проекта";
        var expected = "id_project_документация_проекта";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenMultipleUnderscores_ReturnsCleanedString()
    {
        // Arrange
        var input = "ID___проект____документация";
        var expected = "id_проект_документация";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion

    #region Сценарии с числами и специальными символами

    [Fact]
    public void ProcessDocumentSubject_WhenStringContainsNumbers_ReturnsStringWithNumbers()
    {
        // Arrange
        var input = "Отчет ID проекта 2024";
        var expected = "id_отчет_проекта_2024";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion

    #region Производительность и стабильность

    [Fact]
    public void ProcessDocumentSubject_WhenVeryLongString_ReturnsProcessedString()
    {
        // Arrange
        var input = "Очень длинное название документа с ID которое содержит много слов и должно быть обработано корректно " +
                   "даже если оно содержит несколько ID в разных местах текста ID документа";
        var expected = "id_очень_длинное_название_документа_с_которое_содержит_много_слов_и_должно_быть_обработано_корректно_" +
                      "даже_если_оно_содержит_несколько_в_разных_местах_текста_документа";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenStringWithOnlySeparators_ReturnsEmptyString()
    {
        // Arrange
        var input = "___   ___";
        var expected = string.Empty;

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion

    #region Сценарии с частичным совпадением "id"

    [Fact]
    public void ProcessDocumentSubject_WhenWordStartsWithId_DoesNotRemoveIdPart()
    {
        // Arrange
        var input = "idol документ идея проекта";
        var expected = "id_idol_документ_идея_проекта";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenWordEndsWithId_DoesNotRemoveIdPart()
    {
        // Arrange
        var input = "документ valid проекта";
        var expected = "id_документ_valid_проекта";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenWordContainsIdInMiddle_DoesNotRemoveIdPart()
    {
        // Arrange
        var input = "документ video проекта";
        var expected = "id_документ_video_проекта";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessDocumentSubject_WhenMultipleWordsContainIdPart_ProcessesCorrectly()
    {
        // Arrange
        var input = "identity документ video проекта";
        var expected = "id_identity_документ_video_проекта";

        // Act
        var result = DocumentStringProcessor.ProcessDocumentSubject(input);

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion
}