using TaskManager.Domain.Model.Documents;

namespace TaskManager.Application.Services.Interfaces;

public interface IExportService
{
    byte[] ConvertDocumentToCsv(DocumentForCsvExportModel documentForExportModel);
}