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
    /// Получает список документов с поддержкой расширенной фильтрации, поиска и пагинации.
    /// </summary>
    /// <param name="searchTerm">Поисковый запрос для фильтрации документов по текстовым полям. Применяется, если не null и не пустая строка.</param>
    /// <param name="startOutgoingDocumentDateOutputDocument">Начальная дата диапазона для фильтрации по дате исходящего документа. Включительно.</param>
    /// <param name="endOutgoingDocumentDateOutputDocument">Конечная дата диапазона для фильтрации по дате исходящего документа. Включительно.</param>
    /// <param name="idResponsibleEmployeeInputDocument">Идентификатор ответственного сотрудника для фильтрации документов по ответственному лицу. Если null - фильтр не применяется.</param>
    /// <param name="countSkip">Количество документов для пропуска (используется для пагинации, OFFSET в SQL).</param>
    /// <param name="countTake">Количество документов для выборки (используется для пагинации, LIMIT в SQL).</param>
    /// <param name="documentStatus">Статус документов для фильтрации (активные или архивные). Определяет видимость документов в зависимости от прав доступа.</param>
    /// <returns>
    /// Кортеж, содержащий:
    /// - Список моделей документов для обзора, соответствующих фильтрам и пагинации
    /// - Общее количество документов, соответствующих фильтрам (без учета пагинации)
    /// </returns>
    /// <remarks>
    /// Метод поддерживает комбинированную фильтрацию:
    /// - Фильтр по датам применяется только если заданы обе границы диапазона
    /// - Поиск выполняется по основным текстовым полям документов
    /// - Фильтр по ответственному сотруднику работает независимо от других фильтров
    /// - Для администраторов документы с разными статусами могут обрабатываться особым образом
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">Выбрасывается, если countSkip или countTake имеют отрицательные значения.</exception>
    /// <exception cref="ArgumentException">Выбрасывается, если startOutgoingDocumentDateOutputDocument больше endOutgoingDocumentDateOutputDocument.</exception>
    Task<(List<DocumentForOverviewModel> documents, int countDocuments)> GetDocumentsAsync(
        string? searchTerm,
        DateOnly? startOutgoingDocumentDateOutputDocument,
        DateOnly? endOutgoingDocumentDateOutputDocument,
        int? idResponsibleEmployeeInputDocument,
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