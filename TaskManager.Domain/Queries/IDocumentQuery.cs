using TaskManager.Domain.Enums;
using TaskManager.Domain.Model.Documents;

namespace TaskManager.Domain.Queries;

/// <summary>
/// Интерфейс для выполнения запросов к документам в системе документооборота.
/// Определяет контракты для получения документов в различных представлениях и состояниях.
/// Интерфейс служит абстракцией для взаимодействия с хранилищем данных (базой данных).
/// </summary>
public interface IDocumentQuery
{
    /// <summary>
    /// Получает список документов с поддержкой поиска, пагинации и фильтрации по статусу.
    /// </summary>
    /// <param name="searchTerm">Поисковый запрос для фильтрации документов по текстовым полям</param>
    /// <param name="countSkip">Количество документов для пропуска (используется для пагинации)</param>
    /// <param name="countTake">Количество документов для выборки (используется для пагинации)</param>
    /// <param name="documentStatus">Статус документов для фильтрации (активные или архивные)</param>
    /// <returns>Кортеж, содержащий список моделей документов для обзора и общее количество найденных документов</returns>
    Task<(List<DocumentForOverviewModel> documents, int countDocuments)> GetDocumentsAsync(
        string? searchTerm,
        int countSkip,
        int countTake,
        DocumentStatus documentStatus);

    /// <summary>
    /// Получает документ по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор документа</param>
    /// <returns>Модель документа для редактирования или null, если документ не найден</returns>
    Task<DocumentForEditModel?> GetDocumentForEditAsync(int id);

    /// <summary>
    /// Получает документ по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор документа</param>
    /// <returns>Модель документа для удаления или null, если документ не найден</returns>
    Task<DocumentForDeleteModel?> GetDocumentForDeleteAsync(int id);

    /// <summary>
    /// Получает документ по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор документа</param>
    /// <returns>Модель документа для изменения статуса или null, если документ не найден</returns>
    Task<DocumentForChangeStatusModel?> GetDocumentForChangeStatusAsync(int id);

    /// <summary>
    /// Получает документ по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор документа</param>
    /// <returns>Модель документа для экспорта в CSV или null, если документ не найден</returns>
    Task<DocumentForCsvExportModel?> GetDocumentForCsvExportAsync(int id);

    /// <summary>
    /// Получает идентификатор сотрудника, создавшего документ.
    /// </summary>
    /// <param name="id">Идентификатор документа</param>
    /// <returns>Идентификатор сотрудника-создателя или null, если документ не найден</returns>
    Task<int?> GetIdEmployeeCreatedAsync(int id);

    /// <summary>
    /// Получает идентификатор сотрудника, удалившего документ.
    /// </summary>
    /// <param name="id">Идентификатор документа</param>
    /// <returns>Идентификатор сотрудника-удалителя или null, если документ не найден или не удален</returns>
    Task<int?> GetIdEmployeeRemovedAsync(int id);
}