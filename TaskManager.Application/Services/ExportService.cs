using System.Text;
using TaskManager.Application.Services.Interfaces;
using TaskManager.Domain.Model.Documents;
using TaskManager.Domain.Utils;

namespace TaskManager.Application.Services;

public class ExportService : IExportService
{
    public byte[] ConvertDocumentToCsv(DocumentForCsvExportModel document)
    {
        ArgumentNullException.ThrowIfNull(document);

        var csvBuilder = new StringBuilder();
        csvBuilder.Append('\uFEFF');

        AddHeader(csvBuilder);
        AddInputDataSection(csvBuilder, document);
        AddOutputDataSection(csvBuilder, document);
        AddStatusSection(csvBuilder, document);
        AddMetadataSection(csvBuilder);

        return Encoding.UTF8.GetBytes(csvBuilder.ToString());
    }

    private static void AddHeader(StringBuilder csvBuilder)
    {
        csvBuilder.AppendLine("Отчет по документу");
        csvBuilder.AppendLine();
    }

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

    private static void AddStatusSection(StringBuilder csvBuilder, DocumentForCsvExportModel document)
    {
        csvBuilder.AppendLine("=== СТАТУС ===");
        csvBuilder.AppendLine($"На контроле;{FormatBool(document.IsUnderControl)}");
        csvBuilder.AppendLine($"Завершено;{FormatBool(document.IsCompleted)}");
        csvBuilder.AppendLine($"ФИО создателя;{EscapeCsvField(document.FullNameCreatedEmployee)}");
        csvBuilder.AppendLine();
    }

    private static void AddMetadataSection(StringBuilder csvBuilder)
    {
        csvBuilder.AppendLine($"Отчет сгенерирован;{DateTime.Now.ToString(DateFormatDictionary.DateTimeFormatDdMmYyyyHhMm)}");
    }

    private static string EscapeCsvField(string? field)
    {
        if (string.IsNullOrEmpty(field))
        {
            return string.Empty;
        }

        if (field.Contains(';') || field.Contains('"') || field.Contains('\n') || field.Contains('\r'))
        {
            return $"\"{field.Replace("\"", "\"\"")}\"";
        }

        return field;
    }

    private static string FormatDate(DateOnly? date)
    {
        if (!date.HasValue)
        {
            return string.Empty;
        }

        return date.Value.ToString(DateFormatDictionary.DateFormatDdMmYyyy);
    }

    private static string FormatBool(bool value)
    {
        if (value)
        {
            return "Да";
        }

        return "Нет";
    }
}