using System.Text;
using TaskManager.Application.Services.Interfaces;
using TaskManager.Domain.Model.Documents;
using TaskManager.Domain.Utilities;

namespace TaskManager.Application.Services;

/// <summary>
/// Реализация сервиса для экспорта данных.
/// </summary>
/// <remarks>
/// Обеспечивает преобразование данных в различные форматы для экспорта,
/// включая CSV с поддержкой кириллицы и правильным экранированием полей.
/// </remarks>
public class ExportService : IExportService
{
    /// <summary>
    /// Преобразует данные документа в CSV формат.
    /// </summary>
    /// <param name="document">Модель документа для экспорта.</param>
    /// <returns>Массив байтов, представляющий CSV-файл в кодировке UTF-8 с BOM.</returns>
    /// <exception cref="ArgumentNullException">Выбрасывается, если переданная модель документа равна null.</exception>
    public byte[] ConvertDocumentToCsv(DocumentForCsvExportModel document)
    {
        ArgumentNullException.ThrowIfNull(document, nameof(document));

        var csvBuilder = new StringBuilder();

        // Добавляем BOM для корректного отображения кириллицы в Excel
        csvBuilder.Append('\uFEFF');

        AddHeader(csvBuilder);
        AddInputDataSection(csvBuilder, document);
        AddOutputDataSection(csvBuilder, document);
        AddStatusSection(csvBuilder, document);
        AddMetadataSection(csvBuilder);

        return Encoding.UTF8.GetBytes(csvBuilder.ToString());
    }

    /// <summary>
    /// Добавляет заголовок отчета в CSV.
    /// </summary>
    /// <param name="csvBuilder">StringBuilder для построения CSV.</param>
    private static void AddHeader(StringBuilder csvBuilder)
    {
        csvBuilder.AppendLine("Отчет по документу");
        csvBuilder.AppendLine();
    }

    /// <summary>
    /// Добавляет секцию входных данных документа в CSV.
    /// </summary>
    /// <param name="csvBuilder">StringBuilder для построения CSV.</param>
    /// <param name="document">Модель документа для экспорта.</param>
    private static void AddInputDataSection(StringBuilder csvBuilder, DocumentForCsvExportModel document)
    {
        csvBuilder.AppendLine("=== ВХОДНЫЕ ДАННЫЕ ===");
        csvBuilder.AppendLine($"Исходящий номер (входной);{EscapeCsvField(document.OutgoingDocumentNumberInputDocument)}");
        csvBuilder.AppendLine($"Дата исходного документа;{FormatDate(document.SourceDocumentDateInputDocument)}");
        csvBuilder.AppendLine($"Заказчик;{EscapeCsvField(document.CustomerInputDocument)}");
        csvBuilder.AppendLine($"Краткое содержание (входное);{EscapeCsvField(document.DocumentSummaryInputDocument)}");
        csvBuilder.AppendLine($"Внешний документ (входной);{FormatBool(document.IsExternalDocumentInputDocument)}");
        csvBuilder.AppendLine($"Входящий номер ВХ(46 ЦНИИ);{EscapeCsvField(document.IncomingDocumentNumberInputDocument)}");
        csvBuilder.AppendLine($"Дата входящего документа;{FormatDate(document.IncomingDocumentDateInputDocument)}");
        csvBuilder.AppendLine($"Ответственные отделы;{EscapeCsvField(document.ResponsibleDepartmentsInputDocument)}");
        csvBuilder.AppendLine($"Срок выполнения задачи;{FormatDate(document.TaskDueDateInputDocument)}");
        csvBuilder.AppendLine($"Ответственный сотрудник;{EscapeCsvField(document.FullNameResponsibleEmployeeInputDocument)}");
        csvBuilder.AppendLine();
    }

    /// <summary>
    /// Добавляет секцию выходных данных документа в CSV.
    /// </summary>
    /// <param name="csvBuilder">StringBuilder для построения CSV.</param>
    /// <param name="document">Модель документа для экспорта.</param>
    private static void AddOutputDataSection(StringBuilder csvBuilder, DocumentForCsvExportModel document)
    {
        csvBuilder.AppendLine("=== ВЫХОДНЫЕ ДАННЫЕ ===");
        csvBuilder.AppendLine($"Внешний документ (выходной);{FormatBool(document.IsExternalDocumentOutputDocument)}");
        csvBuilder.AppendLine($"Исходящий номер Исх(46 ЦНИИ);{EscapeCsvField(document.OutgoingDocumentNumberOutputDocument)}");
        csvBuilder.AppendLine($"Дата исходящего документа;{FormatDate(document.OutgoingDocumentDateOutputDocument)}");
        csvBuilder.AppendLine($"Получатель;{EscapeCsvField(document.RecipientOutputDocument)}");
        csvBuilder.AppendLine($"Краткое содержание (выходное);{EscapeCsvField(document.DocumentSummaryOutputDocument)}");
        csvBuilder.AppendLine();
    }

    /// <summary>
    /// Добавляет секцию статуса документа в CSV.
    /// </summary>
    /// <param name="csvBuilder">StringBuilder для построения CSV.</param>
    /// <param name="document">Модель документа для экспорта.</param>
    private static void AddStatusSection(StringBuilder csvBuilder, DocumentForCsvExportModel document)
    {
        csvBuilder.AppendLine("=== СТАТУС ===");
        csvBuilder.AppendLine($"На контроле;{FormatBool(document.IsUnderControl)}");
        csvBuilder.AppendLine($"Завершено;{FormatBool(document.IsCompleted)}");
        csvBuilder.AppendLine($"ФИО создателя;{EscapeCsvField(document.FullNameCreatedEmployee)}");
        csvBuilder.AppendLine();
    }

    /// <summary>
    /// Добавляет метаданные отчета в CSV.
    /// </summary>
    /// <param name="csvBuilder">StringBuilder для построения CSV.</param>
    private static void AddMetadataSection(StringBuilder csvBuilder)
    {
        csvBuilder.AppendLine($"Отчет сгенерирован;{DateTime.Now.ToString(DateFormatDictionary.DateTimeFormatDdMmYyyyHhMm)}");
    }

    /// <summary>
    /// Экранирует поле CSV согласно RFC 4180.
    /// </summary>
    /// <param name="field">Поле для экранирования.</param>
    /// <returns>Экранированная строка поля CSV.</returns>
    /// <remarks>
    /// Правила экранирования:
    /// 1. Пустые строки возвращаются как пустые поля
    /// 2. Поля, содержащие разделители (;), кавычки ("), или символы новой строки, обрамляются кавычками
    /// 3. Кавычки внутри поля удваиваются
    /// </remarks>
    private static string EscapeCsvField(string? field)
    {
        if (string.IsNullOrEmpty(field))
        {
            return string.Empty;
        }

        // Проверяем, нужно ли экранировать поле
        if (field.Contains(';') || field.Contains('"') ||
            field.Contains(Environment.NewLine) ||
            field.Contains('\r') ||
            field.Contains('\n'))
        {
            // Обрамляем поле в кавычки и удваиваем кавычки внутри
            return $"\"{field.Replace("\"", "\"\"")}\"";
        }

        return field;
    }

    /// <summary>
    /// Форматирует дату для отображения в CSV.
    /// </summary>
    /// <param name="date">Дата для форматирования.</param>
    /// <returns>Отформатированная дата или пустая строка, если дата не указана.</returns>
    private static string FormatDate(DateOnly? date)
    {
        if (!date.HasValue)
        {
            return string.Empty;
        }

        return date.Value.ToString(DateFormatDictionary.DateFormatDdMmYyyy);
    }

    /// <summary>
    /// Форматирует булево значение для отображения в CSV.
    /// </summary>
    /// <param name="value">Булево значение для форматирования.</param>
    /// <returns>"Да" для true, "Нет" для false.</returns>
    private static string FormatBool(bool value)
    {
        if (value)
        {
            return "Да";
        }

        return "Нет";
    }
}