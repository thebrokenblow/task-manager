using TaskManager.Domain.Model.Documents;

namespace TaskManager.Domain.Queries;

/// <summary>
/// Предоставляет запросы для работы с данными документов.
/// Реализует сложные сценарии чтения данных с фильтрацией, пагинацией.
/// </summary>
public interface IDocumentQuery
{
    /// <summary>
    /// Получает список документов с фильтрацией, пагинацией и подсчетом общего количества.
    /// </summary>
    /// <param name="documentFilterModel">Модель фильтрации документов.</param>
    /// <param name="countSkip">Количество записей для пропуска (пагинация).</param>
    /// <param name="countTake">Количество записей для получения (пагинация).</param>
    /// <returns>
    /// Задача, результат которой содержит кортеж с двумя значениями:
    /// - Перечисление моделей <see cref="DocumentForOverviewModel"/> с примененными фильтрами
    /// - Общее количество документов, соответствующих фильтрам (до применения пагинации)
    /// </returns>
    Task<(IEnumerable<DocumentForOverviewModel> documents, int countDocuments)> GetDocumentsAsync(
        DocumentFilterModel documentFilterModel,
        int countSkip,
        int countTake);

    /// <summary>
    /// Получает данные документа для редактирования.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    /// <returns>
    /// Задача, результат которой содержит модель <see cref="DocumentForOverviewEditModel"/> 
    /// или <c>null</c>, если документ не найден.
    /// </returns>
    /// <remarks>
    /// Запрос возвращает все поля документа, необходимые для формы редактирования.
    /// </remarks>
    Task<DocumentForOverviewEditModel?> GetDocumentForEditAsync(int id);

    /// <summary>
    /// Получает данные документа для подтверждения удаления.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    /// <returns>
    /// Задача, результат которой содержит модель <see cref="DocumentForDeleteModel"/> 
    /// или <c>null</c>, если документ не найден.
    /// </returns>
    /// <remarks>
    /// Запрос возвращает основные поля документа, необходимые для отображения перед удалением.
    /// </remarks>
    Task<DocumentForDeleteModel?> GetDocumentForDeleteAsync(int id);

    /// <summary>
    /// Получает идентификатор сотрудника, который удалил документ.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    /// <returns>
    /// Задача, результат которой содержит идентификатор сотрудника, удалившего документ,
    /// или <c>null</c>, если документ не удален или не найден.
    /// </returns>
    Task<int?> GetIdEmployeeRemovedAsync(int id);

    /// <summary>
    /// Получает идентификатор сотрудника, который создал документ.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    /// <returns>
    /// Задача, результат которой содержит идентификатор сотрудника, создавшего документ,
    /// или <c>null</c>, если документ не найден.
    /// </returns>
    Task<int?> GetIdEmployeeCreatedAsync(int id);

    /// <summary>
    /// Получает данные документа для изменения статуса завершения.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    /// <returns>
    /// Задача, результат которой содержит модель <see cref="DocumentForChangeStatusModel"/> 
    /// или <c>null</c>, если документ не найден.
    /// </returns>
    /// <remarks>
    /// Возвращает только поля, которые должны быть заполнены перед закрытием документа.
    /// </remarks>
    Task<DocumentForChangeStatusModel?> GetDocumentForChangeStatusAsync(int id);

    /// <summary>
    /// Получает данные документа для экспорта в CSV-формат.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    /// <returns>
    /// Задача, результат которой содержит модель <see cref="DocumentForCsvExportModel"/> 
    /// или <c>null</c>, если документ не найден.
    /// </returns>
    /// <remarks>
    /// Возвращает все поля документа, необходимые для формирования CSV-файла.
    /// </remarks>
    Task<DocumentForCsvExportModel?> GetDocumentForCsvExportAsync(int id);
}