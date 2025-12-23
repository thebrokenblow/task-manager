using TaskManager.Domain.Model.Documents.Query;
using TaskManager.UnitTests.Stubs;
using TaskManager.View.Utilities;

namespace TaskManager.UnitTests.Utilities;

/// <summary>
/// Набор unit-тестов для класса <see cref="DocumentStyler"/>.
/// </summary>
/// <remarks>
/// Тесты покрывают все основные сценарии стилизации документов:
/// 1. Определение цвета строки на основе срока выполнения
/// 2. Выделение документов на контроле
/// 3. Проверка корректности констант
/// </remarks>
public class DocumentStylerTests
{
    private readonly DocumentStyler documentStyler = new(new AuthServiceStub());
    private readonly DateOnly today = DateOnly.FromDateTime(DateTime.Now);

    #region DetermineRowColor Tests

    /// <summary>
    /// Проверяет, что при сроке выполнения сегодня возвращается класс TableDanger.
    /// </summary>
    [Fact]
    public void DetermineRowColor_WhenDueDateIsToday_ShouldReturnTableDanger()
    {
        // Arrange
        var document = new DocumentForOverviewModel
        {
            Id = 1,
            OutgoingDocumentNumberInputDocument = "Исх-123/2024",
            DocumentSummaryInputDocument = "Тестовый документ",
            IncomingDocumentNumberInputDocument = "Вх-456/2024",
            CustomerInputDocument = "ООО Ромашка",
            TaskDueDateInputDocument = today,
            OutgoingDocumentNumberOutputDocument = "Исх-789/2024",
            OutgoingDocumentDateOutputDocument = today,
            IsUnderControl = false,
            FullNameResponsibleEmployee = "Иванов Иван Иванович",
            CreatedByEmployeeId = 1,
            IsCompleted = false,
            RemovedByEmployeeId = null,
            RemoveDateTime = null
        };

        // Act
        var result = documentStyler.DetermineRowColor(document);

        // Assert
        Assert.Equal(TableCssClassDictionary.TableDanger, result);
    }

    /// <summary>
    /// Проверяет, что при просроченном сроке (вчера) возвращается класс TableDanger.
    /// </summary>
    [Fact]
    public void DetermineRowColor_WhenDueDateWasYesterday_ShouldReturnTableDanger()
    {
        // Arrange
        var document = new DocumentForOverviewModel
        {
            Id = 2,
            OutgoingDocumentNumberInputDocument = "Исх-124/2024",
            DocumentSummaryInputDocument = "Документ с просроченным сроком",
            IncomingDocumentNumberInputDocument = "Вх-457/2024",
            CustomerInputDocument = "ЗАО Лилия",
            TaskDueDateInputDocument = today.AddDays(-1),
            OutgoingDocumentNumberOutputDocument = "Исх-790/2024",
            OutgoingDocumentDateOutputDocument = today,
            IsUnderControl = true,
            FullNameResponsibleEmployee = "Петров Петр Петрович",
            CreatedByEmployeeId = 2,
            IsCompleted = false,
            RemovedByEmployeeId = null,
            RemoveDateTime = null
        };

        // Act
        var result = documentStyler.DetermineRowColor(document);

        // Assert
        Assert.Equal(TableCssClassDictionary.TableDanger, result);
    }

    /// <summary>
    /// Проверяет, что при сроке выполнения через 2 дня возвращается класс TableWarning.
    /// </summary>
    [Fact]
    public void DetermineRowColor_WhenDueDateIn2Days_ShouldReturnTableWarning()
    {
        // Arrange
        var document = new DocumentForOverviewModel
        {
            Id = 3,
            OutgoingDocumentNumberInputDocument = "Исх-125/2024",
            DocumentSummaryInputDocument = "Срочный документ",
            IncomingDocumentNumberInputDocument = "Вх-458/2024",
            CustomerInputDocument = "АО Тюльпан",
            TaskDueDateInputDocument = today.AddDays(2),
            OutgoingDocumentNumberOutputDocument = "Исх-791/2024",
            OutgoingDocumentDateOutputDocument = today,
            IsUnderControl = true,
            FullNameResponsibleEmployee = "Сидоров Сидор Сидорович",
            CreatedByEmployeeId = 3,
            IsCompleted = false,
            RemovedByEmployeeId = null,
            RemoveDateTime = null
        };

        // Act
        var result = documentStyler.DetermineRowColor(document);

        // Assert
        Assert.Equal(TableCssClassDictionary.TableWarning, result);
    }

    /// <summary>
    /// Проверяет, что при сроке выполнения через 3 дня возвращается класс TableWarning.
    /// </summary>
    [Fact]
    public void DetermineRowColor_WhenDueDateIn3Days_ShouldReturnTableWarning()
    {
        // Arrange
        var document = new DocumentForOverviewModel
        {
            Id = 4,
            OutgoingDocumentNumberInputDocument = "Исх-126/2024",
            DocumentSummaryInputDocument = "Документ требующий внимания",
            IncomingDocumentNumberInputDocument = "Вх-459/2024",
            CustomerInputDocument = "ИП Васильев",
            TaskDueDateInputDocument = today.AddDays(3),
            OutgoingDocumentNumberOutputDocument = "Исх-792/2024",
            OutgoingDocumentDateOutputDocument = today,
            IsUnderControl = false,
            FullNameResponsibleEmployee = "Кузнецова Анна Сергеевна",
            CreatedByEmployeeId = 4,
            IsCompleted = true,
            RemovedByEmployeeId = null,
            RemoveDateTime = null
        };

        // Act
        var result = documentStyler.DetermineRowColor(document);

        // Assert
        Assert.Equal(TableCssClassDictionary.TableWarning, result);
    }

    /// <summary>
    /// Проверяет, что при сроке выполнения через 5 дней возвращается класс TableSuccess.
    /// </summary>
    [Fact]
    public void DetermineRowColor_WhenDueDateIn5Days_ShouldReturnTableSuccess()
    {
        // Arrange
        var document = new DocumentForOverviewModel
        {
            Id = 5,
            OutgoingDocumentNumberInputDocument = "Исх-127/2024",
            DocumentSummaryInputDocument = "Документ в работе",
            IncomingDocumentNumberInputDocument = "Вх-460/2024",
            CustomerInputDocument = "ООО Подсолнух",
            TaskDueDateInputDocument = today.AddDays(5),
            OutgoingDocumentNumberOutputDocument = "Исх-793/2024",
            OutgoingDocumentDateOutputDocument = today.AddDays(1),
            IsUnderControl = false,
            FullNameResponsibleEmployee = "Морозова Елена Владимировна",
            CreatedByEmployeeId = 5,
            IsCompleted = false,
            RemovedByEmployeeId = null,
            RemoveDateTime = null
        };

        // Act
        var result = documentStyler.DetermineRowColor(document);

        // Assert
        Assert.Equal(TableCssClassDictionary.TableSuccess, result);
    }

    /// <summary>
    /// Проверяет, что при сроке выполнения через 7 дней возвращается класс TableSuccess.
    /// </summary>
    [Fact]
    public void DetermineRowColor_WhenDueDateIn7Days_ShouldReturnTableSuccess()
    {
        // Arrange
        var document = new DocumentForOverviewModel
        {
            Id = 6,
            OutgoingDocumentNumberInputDocument = "Исх-128/2024",
            DocumentSummaryInputDocument = "Плановый документ",
            IncomingDocumentNumberInputDocument = "Вх-461/2024",
            CustomerInputDocument = "ГК Орхидея",
            TaskDueDateInputDocument = today.AddDays(7),
            OutgoingDocumentNumberOutputDocument = "Исх-794/2024",
            OutgoingDocumentDateOutputDocument = today.AddDays(2),
            IsUnderControl = true,
            FullNameResponsibleEmployee = "Новиков Дмитрий Александрович",
            CreatedByEmployeeId = 6,
            IsCompleted = true,
            RemovedByEmployeeId = null,
            RemoveDateTime = null
        };

        // Act
        var result = documentStyler.DetermineRowColor(document);

        // Assert
        Assert.Equal(TableCssClassDictionary.TableSuccess, result);
    }

    /// <summary>
    /// Проверяет, что при сроке выполнения через 10 дней возвращается пустая строка (без стиля).
    /// </summary>
    [Fact]
    public void DetermineRowColor_WhenDueDateIn10Days_ShouldReturnEmptyString()
    {
        // Arrange
        var document = new DocumentForOverviewModel
        {
            Id = 7,
            OutgoingDocumentNumberInputDocument = "Исх-129/2024",
            DocumentSummaryInputDocument = "Долгосрочный документ",
            IncomingDocumentNumberInputDocument = "Вх-462/2024",
            CustomerInputDocument = "Холдинг Магнолия",
            TaskDueDateInputDocument = today.AddDays(10),
            OutgoingDocumentNumberOutputDocument = "Исх-795/2024",
            OutgoingDocumentDateOutputDocument = today.AddDays(3),
            IsUnderControl = false,
            FullNameResponsibleEmployee = "Соколова Ольга Игоревна",
            CreatedByEmployeeId = 7,
            IsCompleted = false,
            RemovedByEmployeeId = null,
            RemoveDateTime = null
        };

        // Act
        var result = documentStyler.DetermineRowColor(document);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    /// <summary>
    /// Проверяет, что при сроке выполнения через 30 дней возвращается пустая строка (без стиля).
    /// </summary>
    [Fact]
    public void DetermineRowColor_WhenDueDateIn30Days_ShouldReturnEmptyString()
    {
        // Arrange
        var document = new DocumentForOverviewModel
        {
            Id = 8,
            OutgoingDocumentNumberInputDocument = "Исх-130/2024",
            DocumentSummaryInputDocument = "Перспективный документ",
            IncomingDocumentNumberInputDocument = "Вх-463/2024",
            CustomerInputDocument = "Корпорация Лаванда",
            TaskDueDateInputDocument = today.AddDays(30),
            OutgoingDocumentNumberOutputDocument = "Исх-796/2024",
            OutgoingDocumentDateOutputDocument = today.AddDays(5),
            IsUnderControl = true,
            FullNameResponsibleEmployee = "Волков Артем Сергеевич",
            CreatedByEmployeeId = 8,
            IsCompleted = true,
            RemovedByEmployeeId = null,
            RemoveDateTime = null
        };

        // Act
        var result = documentStyler.DetermineRowColor(document);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    /// <summary>
    /// Проверяет, что при сроке выполнения через 4 дня возвращается класс TableSuccess.
    /// </summary>
    [Fact]
    public void DetermineRowColor_WhenDueDateIn4Days_ShouldReturnTableSuccess()
    {
        // Arrange
        var document = new DocumentForOverviewModel
        {
            Id = 9,
            OutgoingDocumentNumberInputDocument = "Исх-131/2024",
            DocumentSummaryInputDocument = "Документ в работе",
            IncomingDocumentNumberInputDocument = "Вх-464/2024",
            CustomerInputDocument = "ООО Жасмин",
            TaskDueDateInputDocument = today.AddDays(4),
            OutgoingDocumentNumberOutputDocument = "Исх-797/2024",
            OutgoingDocumentDateOutputDocument = today,
            IsUnderControl = false,
            FullNameResponsibleEmployee = "Павлова Мария Дмитриевна",
            CreatedByEmployeeId = 9,
            IsCompleted = false,
            RemovedByEmployeeId = null,
            RemoveDateTime = null
        };

        // Act
        var result = documentStyler.DetermineRowColor(document);

        // Assert
        Assert.Equal(TableCssClassDictionary.TableSuccess, result);
    }

    /// <summary>
    /// Проверяет, что при сроке выполнения через 6 дней возвращается класс TableSuccess.
    /// </summary>
    [Fact]
    public void DetermineRowColor_WhenDueDateIn6Days_ShouldReturnTableSuccess()
    {
        // Arrange
        var document = new DocumentForOverviewModel
        {
            Id = 10,
            OutgoingDocumentNumberInputDocument = "Исх-132/2024",
            DocumentSummaryInputDocument = "Документ в работе",
            IncomingDocumentNumberInputDocument = "Вх-465/2024",
            CustomerInputDocument = "ИП Ковалев",
            TaskDueDateInputDocument = today.AddDays(6),
            OutgoingDocumentNumberOutputDocument = "Исх-798/2024",
            OutgoingDocumentDateOutputDocument = today,
            IsUnderControl = true,
            FullNameResponsibleEmployee = "Федоров Алексей Викторович",
            CreatedByEmployeeId = 10,
            IsCompleted = true,
            RemovedByEmployeeId = null,
            RemoveDateTime = null
        };

        // Act
        var result = documentStyler.DetermineRowColor(document);

        // Assert
        Assert.Equal(TableCssClassDictionary.TableSuccess, result);
    }

    #endregion

    #region GetControlHighlight Tests

    /// <summary>
    /// Проверяет, что для документа на контроле возвращается класс FontWeightBold.
    /// </summary>
    [Fact]
    public void GetControlHighlight_WhenDocumentIsUnderControl_ShouldReturnFontWeightBold()
    {
        // Arrange
        var document = new DocumentForOverviewModel
        {
            Id = 11,
            OutgoingDocumentNumberInputDocument = "Исх-133/2024",
            DocumentSummaryInputDocument = "Документ под контролем",
            IncomingDocumentNumberInputDocument = "Вх-466/2024",
            CustomerInputDocument = "ООО Астра",
            TaskDueDateInputDocument = today.AddDays(5),
            OutgoingDocumentNumberOutputDocument = "Исх-799/2024",
            OutgoingDocumentDateOutputDocument = today,
            IsUnderControl = true,
            FullNameResponsibleEmployee = "Григорьева Татьяна Олеговна",
            CreatedByEmployeeId = 11,
            IsCompleted = false,
            RemovedByEmployeeId = null,
            RemoveDateTime = null
        };

        // Act
        var result = documentStyler.GetControlHighlight(document);

        // Assert
        Assert.Equal(CssClassDictionary.FontWeightBold, result);
    }

    /// <summary>
    /// Проверяет, что для документа не на контроле возвращается пустая строка.
    /// </summary>
    [Fact]
    public void GetControlHighlight_WhenDocumentIsNotUnderControl_ShouldReturnEmptyString()
    {
        // Arrange
        var document = new DocumentForOverviewModel
        {
            Id = 12,
            OutgoingDocumentNumberInputDocument = "Исх-134/2024",
            DocumentSummaryInputDocument = "Обычный документ",
            IncomingDocumentNumberInputDocument = "Вх-467/2024",
            CustomerInputDocument = "ЗАО Пион",
            TaskDueDateInputDocument = today.AddDays(8),
            OutgoingDocumentNumberOutputDocument = "Исх-800/2024",
            OutgoingDocumentDateOutputDocument = today,
            IsUnderControl = false,
            FullNameResponsibleEmployee = "Белов Игорь Николаевич",
            CreatedByEmployeeId = 12,
            IsCompleted = true,
            RemovedByEmployeeId = null,
            RemoveDateTime = null
        };

        // Act
        var result = documentStyler.GetControlHighlight(document);

        // Assert
        Assert.Equal(string.Empty, result);
    }

    #endregion

    #region Constants Tests

    /// <summary>
    /// Проверяет корректность значений констант в классе DocumentStyler.
    /// </summary>
    [Fact]
    public void Constants_ShouldHaveCorrectValues()
    {
        // Assert
        Assert.Equal(0, DocumentStyler.OverdueDay);
        Assert.Equal(3, DocumentStyler.WarningDay);
        Assert.Equal(7, DocumentStyler.NormalDay);
    }

    #endregion
}