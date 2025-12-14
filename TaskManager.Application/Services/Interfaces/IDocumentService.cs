using TaskManager.Application.Common;
using TaskManager.Application.Dtos.Documents;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Model.Documents;

namespace TaskManager.Application.Services.Interfaces;

/// <summary>
/// Сервис для управления документами.
/// </summary>
public interface IDocumentService
{
    /// <summary>
    /// Получает постраничный список документов.
    /// </summary>
    /// <param name="documentFilterModel">Модель фильтрации документов.</param>
    /// <param name="page">Номер страницы.</param>
    /// <param name="pageSize">Размер страницы.</param>
    Task<PagedResult<DocumentForOverviewModel>> GetPagedAsync(
        DocumentFilterDto documentFilterModel,
        int page,
        int pageSize);

    /// <summary>
    /// Получает документ по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    Task<Document?> GetByIdAsync(int id);

    /// <summary>
    /// Получает данные документа для редактирования.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    Task<DocumentForEditModel?> GetDocumentForEditAsync(int id);

    /// <summary>
    /// Получает данные документа для удаления.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    Task<DocumentForDeleteModel?> GetDocumentForDeleteAsync(int id);

    /// <summary>
    /// Создает новый документ.
    /// </summary>
    /// <param name="document">Документ для создания.</param>
    Task CreateAsync(Document document);

    /// <summary>
    /// Редактирует существующий документ.
    /// </summary>
    /// <param name="document">Документ с обновленными данными.</param>
    Task EditAsync(Document document);

    /// <summary>
    /// Удаляет документ.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Восстанавливает удаленный документ.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    Task RecoverDeletedAsync(int id);

    /// <summary>
    /// Изменяет статус завершения документа.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    Task ChangeStatusAsync(int id);

    /// <summary>
    /// Создает CSV-файл с данными документа.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    Task<byte[]> CreateDocumentCsvAsync(int id);
}