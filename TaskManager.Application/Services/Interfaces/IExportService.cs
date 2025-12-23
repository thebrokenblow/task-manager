using TaskManager.Domain.Model.Documents.Query;

namespace TaskManager.Application.Services.Interfaces;

/// <summary>
/// Сервис для экспорта данных в различные форматы.
/// </summary>
public interface IExportService
{
    /// <summary>
    /// Преобразует данные документа в CSV формат.
    /// </summary>
    /// <param name="documentForExportModel">Модель документа для экспорта.</param>
    /// <returns>Массив байтов, представляющий CSV-файл в кодировке UTF-8 с BOM.</returns>
    byte[] ConvertDocumentToCsv(DocumentForOverviewCsvExportModel documentForExportModel);
}