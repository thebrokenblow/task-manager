using TaskManager.Application.Utilities;

namespace TaskManager.UnitTests.Utilities;

/// <summary>
/// Набор unit-тестов для класса <see cref="EmployeeStringProcessor"/>.
/// </summary>
/// <remarks>
/// Тесты покрывают все основные сценарии обработки строк для сотрудников:
/// 1. Очистка лишних пробелов
/// 2. Конвертация пробелов в нижние подчеркивания
/// 3. Обработка пограничных случаев (null, пустые строки, только пробелы)
/// </remarks>
public class EmployeeStringProcessorTests
{
    #region CleanSpaces Tests

    /// <summary>
    /// Проверяет, что метод удаляет двойные пробелы между словами.
    /// </summary>
    [Fact]
    public void CleanSpaces_WhenStringHasDoubleSpaces_ShouldReturnSingleSpaces()
    {
        // Arrange
        var input = "Отдел   кадров    и   обучения";
        var expected = "Отдел кадров и обучения";

        // Act
        var result = EmployeeStringProcessor.CleanSpaces(input);

        // Assert
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Проверяет, что метод удаляет пробелы в начале и конце строки.
    /// </summary>
    [Fact]
    public void CleanSpaces_WhenStringHasLeadingAndTrailingSpaces_ShouldTrimSpaces()
    {
        // Arrange
        var input = "   Финансовый отдел   ";
        var expected = "Финансовый отдел";

        // Act
        var result = EmployeeStringProcessor.CleanSpaces(input);

        // Assert
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Проверяет, что метод корректно обрабатывает строку без лишних пробелов.
    /// </summary>
    [Fact]
    public void CleanSpaces_WhenStringHasNoExtraSpaces_ShouldReturnSameString()
    {
        // Arrange
        var input = "Отдел информационных технологий";
        var expected = "Отдел информационных технологий";

        // Act
        var result = EmployeeStringProcessor.CleanSpaces(input);

        // Assert
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Проверяет, что метод возвращает null при null входном значении.
    /// </summary>
    [Fact]
    public void CleanSpaces_WhenInputIsNull_ShouldReturnNull()
    {
        // Arrange
        string? input = null;
        var expected = string.Empty;


        // Act
        var result = EmployeeStringProcessor.CleanSpaces(input);

        // Assert
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Проверяет, что метод возвращает пустую строку при пустой входной строке.
    /// </summary>
    [Fact]
    public void CleanSpaces_WhenInputIsEmptyString_ShouldReturnEmptyString()
    {
        // Arrange
        var input = string.Empty;
        var expected = string.Empty;

        // Act
        var result = EmployeeStringProcessor.CleanSpaces(input);

        // Assert
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Проверяет, что метод возвращает пустую строку при строке из одних пробелов.
    /// </summary>
    [Fact]
    public void CleanSpaces_WhenInputIsOnlySpaces_ShouldReturnEmptyString()
    {
        // Arrange
        var input = "     ";
        var expected = string.Empty;

        // Act
        var result = EmployeeStringProcessor.CleanSpaces(input);

        // Assert
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Проверяет, что метод корректно обрабатывает строку с множественными пробелами.
    /// </summary>
    [Fact]
    public void CleanSpaces_WhenStringHasMultipleSpacesEverywhere_ShouldCleanAllSpaces()
    {
        // Arrange
        var input = "   01    Отдел      разработки      ПО     ";
        var expected = "01 Отдел разработки ПО";

        // Act
        var result = EmployeeStringProcessor.CleanSpaces(input);

        // Assert
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Проверяет обработку английского текста с лишними пробелами.
    /// </summary>
    [Fact]
    public void CleanSpaces_WhenEnglishTextHasExtraSpaces_ShouldReturnCleanText()
    {
        // Arrange
        var input = "  IT   Development    Department   ";
        var expected = "IT Development Department";

        // Act
        var result = EmployeeStringProcessor.CleanSpaces(input);

        // Assert
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Проверяет обработку смешанного русско-английского текста.
    /// </summary>
    [Fact]
    public void CleanSpaces_WhenMixedLanguageTextHasSpaces_ShouldReturnCleanText()
    {
        // Arrange
        var input = "  QA   Department (Отдел тестирования)   ";
        var expected = "QA Department (Отдел тестирования)";

        // Act
        var result = EmployeeStringProcessor.CleanSpaces(input);

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion

    #region ConvertSpacesToUnderscore Tests

    /// <summary>
    /// Проверяет, что метод заменяет пробелы на нижние подчеркивания.
    /// </summary>
    [Fact]
    public void ConvertSpacesToUnderscore_WhenStringHasSpaces_ShouldConvertToUnderscores()
    {
        // Arrange
        var input = "john.doe";
        var expected = "john.doe";

        // Act
        var result = EmployeeStringProcessor.ConvertSpacesToUnderscore(input);

        // Assert
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Проверяет, что метод удаляет лишние пробелы перед конвертацией в подчеркивания.
    /// </summary>
    [Fact]
    public void ConvertSpacesToUnderscore_WhenStringHasExtraSpaces_ShouldCleanAndConvert()
    {
        // Arrange
        var input = "jane   smith   doe";
        var expected = "jane_smith_doe";

        // Act
        var result = EmployeeStringProcessor.ConvertSpacesToUnderscore(input);

        // Assert
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Проверяет, что метод обрабатывает строку без пробелов (возвращает исходную строку).
    /// </summary>
    [Fact]
    public void ConvertSpacesToUnderscore_WhenStringHasNoSpaces_ShouldReturnSameString()
    {
        // Arrange
        var input = "admin";
        var expected = "admin";

        // Act
        var result = EmployeeStringProcessor.ConvertSpacesToUnderscore(input);

        // Assert
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Проверяет, что метод возвращает null при null входном значении.
    /// </summary>
    [Fact]
    public void ConvertSpacesToUnderscore_WhenInputIsNull_ShouldReturnNull()
    {
        // Arrange
        string? input = null;
        var expected = string.Empty;


        // Act
        var result = EmployeeStringProcessor.ConvertSpacesToUnderscore(input);

        // Assert
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Проверяет, что метод возвращает пустую строку при пустой входной строке.
    /// </summary>
    [Fact]
    public void ConvertSpacesToUnderscore_WhenInputIsEmptyString_ShouldReturnEmptyString()
    {
        // Arrange
        var input = string.Empty;
        var expected = string.Empty;

        // Act
        var result = EmployeeStringProcessor.ConvertSpacesToUnderscore(input);

        // Assert
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Проверяет, что метод возвращает пустую строку при строке из одних пробелов.
    /// </summary>
    [Fact]
    public void ConvertSpacesToUnderscore_WhenInputIsOnlySpaces_ShouldReturnEmptyString()
    {
        // Arrange
        var input = "     ";
        var expected = string.Empty;

        // Act
        var result = EmployeeStringProcessor.ConvertSpacesToUnderscore(input);

        // Assert
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Проверяет, что метод обрабатывает пробелы в начале и конце строки.
    /// </summary>
    [Fact]
    public void ConvertSpacesToUnderscore_WhenStringHasLeadingAndTrailingSpaces_ShouldTrimAndConvert()
    {
        // Arrange
        var input = "  robert  johnson  ";
        var expected = "robert_johnson";

        // Act
        var result = EmployeeStringProcessor.ConvertSpacesToUnderscore(input);

        // Assert
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Проверяет, что метод корректно обрабатывает сложную строку с разным количеством пробелов.
    /// </summary>
    [Fact]
    public void ConvertSpacesToUnderscore_WhenStringHasComplexSpacing_ShouldCleanAndConvert()
    {
        // Arrange
        var input = "   system   admin   user   123   ";
        var expected = "system_admin_user_123";

        // Act
        var result = EmployeeStringProcessor.ConvertSpacesToUnderscore(input);

        // Assert
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Проверяет обработку email-подобных логинов.
    /// </summary>
    [Fact]
    public void ConvertSpacesToUnderscore_WhenEmailLikeLogin_ShouldConvertCorrectly()
    {
        // Arrange
        var input = "firstname.lastname company";
        var expected = "firstname.lastname_company";

        // Act
        var result = EmployeeStringProcessor.ConvertSpacesToUnderscore(input);

        // Assert
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Проверяет обработку логина с цифрами.
    /// </summary>
    [Fact]
    public void ConvertSpacesToUnderscore_WhenLoginHasNumbers_ShouldConvertCorrectly()
    {
        // Arrange
        var input = "user 007 agent";
        var expected = "user_007_agent";

        // Act
        var result = EmployeeStringProcessor.ConvertSpacesToUnderscore(input);

        // Assert
        Assert.Equal(expected, result);
    }

    #endregion

    #region Integration Tests

    /// <summary>
    /// Проверяет последовательное применение обоих методов.
    /// </summary>
    [Fact]
    public void CombinedUsage_CleanSpacesThenConvertToUnderscore_ShouldWorkCorrectly()
    {
        // Arrange
        var department = "   02   Отдел   Тестирования   ";
        var login = "  michael  brown  jr  ";

        // Act
        var cleanDepartment = EmployeeStringProcessor.CleanSpaces(department);
        var cleanLogin = EmployeeStringProcessor.CleanSpaces(login);
        var underscoreLogin = EmployeeStringProcessor.ConvertSpacesToUnderscore(login);

        // Assert
        Assert.Equal("02 Отдел Тестирования", cleanDepartment);
        Assert.Equal("michael brown jr", cleanLogin);
        Assert.Equal("michael_brown_jr", underscoreLogin);
    }

    /// <summary>
    /// Проверяет, что ConvertSpacesToUnderscore эквивалентен CleanSpaces с заменой разделителя.
    /// </summary>
    [Fact]
    public void ConvertSpacesToUnderscore_ShouldBeEquivalentToCleanSpacesWithUnderscore()
    {
        // Arrange
        var input = "  test  user  account  ";
        var expectedCleanSpacesResult = "test user account";
        var expectedConvertResult = "test_user_account";

        // Act
        var cleanSpacesResult = EmployeeStringProcessor.CleanSpaces(input);
        var convertResult = EmployeeStringProcessor.ConvertSpacesToUnderscore(input);

        // Assert
        Assert.Equal(expectedCleanSpacesResult, cleanSpacesResult);
        Assert.Equal(expectedConvertResult, convertResult);
    }

    /// <summary>
    /// Проверяет обработку реальных примеров логинов сотрудников.
    /// </summary>
    [Fact]
    public void ConvertSpacesToUnderscore_RealWorldLogins_ShouldFormatCorrectly()
    {
        // Arrange
        var firstNameLastNameInput = "john smith";
        var firstNameLastNameExpected = "john_smith";

        var dotNotationInput = "alex.jones it";
        var dotNotationExpected = "alex.jones_it";

        var nameYearInput = "sarah connor 2024";
        var nameYearExpected = "sarah_connor_2024";

        var multipleSpacesInput = "  super  user  admin  ";
        var multipleSpacesExpected = "super_user_admin";

        // Act
        var firstNameLastNameResult = EmployeeStringProcessor.ConvertSpacesToUnderscore(firstNameLastNameInput);
        var dotNotationResult = EmployeeStringProcessor.ConvertSpacesToUnderscore(dotNotationInput);
        var nameYearResult = EmployeeStringProcessor.ConvertSpacesToUnderscore(nameYearInput);
        var multipleSpacesResult = EmployeeStringProcessor.ConvertSpacesToUnderscore(multipleSpacesInput);

        // Assert
        Assert.Equal(firstNameLastNameExpected, firstNameLastNameResult);
        Assert.Equal(dotNotationExpected, dotNotationResult);
        Assert.Equal(nameYearExpected, nameYearResult);
        Assert.Equal(multipleSpacesExpected, multipleSpacesResult);
    }

    #endregion
}