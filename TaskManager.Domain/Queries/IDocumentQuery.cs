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
    /// Получает отфильтрованный список документов с пагинацией.
    /// </summary>
    /// <param name="documentFilterModel">Параметры фильтрации документов</param>
    /// <param name="countSkip">Количество пропускаемых документов (пагинация)</param>
    /// <param name="countTake">Количество возвращаемых документов (пагинация)</param>
    /// <returns>Список документов и общее количество, соответствующее фильтрам</returns>
    Task<(List<DocumentForOverviewModel> documents, int countDocuments)> GetDocumentsAsync(
        DocumentFilterModel documentFilterModel,
        int countSkip,
        int countTake);

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