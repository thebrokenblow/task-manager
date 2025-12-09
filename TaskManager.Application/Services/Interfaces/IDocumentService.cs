using TaskManager.Application.Common;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Model.Documents;

namespace TaskManager.Application.Services.Interfaces;

/// <summary>
/// Сервис для управления документами.
/// </summary>
/// <remarks>
/// Предоставляет методы для работы с документами, включая получение, создание, редактирование,
/// удаление, восстановление, изменение статуса и экспорт документов.
/// </remarks>
public interface IDocumentService
{

    /// <summary>
    /// Получает постраничный список документов с расширенными параметрами фильтрации.
    /// </summary>
    /// <param name="searchTerm">Термин для поиска по текстовым полям документов. Если null или пустая строка - поиск не применяется.</param>
    /// <param name="showMyTasks">Флаг, указывающий показывать только задачи текущего пользователя (true) или все задачи (false).</param>
    /// <param name="startOutgoingDocumentDateOutputDocument">Начальная дата для фильтрации по дате исходящего документа (включительно). Если null - фильтр не применяется.</param>
    /// <param name="endOutgoingDocumentDateOutputDocument">Конечная дата для фильтрации по дате исходящего документа (включительно). Если null - фильтр не применяется.</param>
    /// <param name="page">Номер текущей страницы (начинается с 1).</param>
    /// <param name="pageSize">Размер страницы - количество элементов на странице.</param>
    /// <returns>Объект <see cref="PagedResult{T}"/> с отфильтрованными и постранично разбитыми документами для отображения.</returns>
    /// <remarks>
    /// Метод поддерживает комбинирование фильтров:
    /// - Поиск по тексту работает независимо от других фильтров
    /// - Фильтр по датам применяется только если заданы обе границы
    /// - Фильтр по задачам пользователя зависит от контекста аутентификации
    /// - Для администраторов могут показываться дополнительные документы
    /// </remarks>
    Task<PagedResult<DocumentForOverviewModel>> GetPagedAsync(
        string? searchTerm,
        bool showMyTasks,
        DateOnly? startOutgoingDocumentDateOutputDocument,
        DateOnly? endOutgoingDocumentDateOutputDocument,
        int page,
        int pageSize);

    /// <summary>
    /// Получает документ по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    /// <returns>Объект документа или null, если документ не найден.</returns>
    Task<Document?> GetByIdAsync(int id);

    /// <summary>
    /// Получает данные документа для редактирования.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    /// <returns>Модель данных для редактирования документа или null, если документ не найден.</returns>
    Task<DocumentForEditModel?> GetDocumentForEditAsync(int id);

    /// <summary>
    /// Получает данные документа для подтверждения удаления.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    /// <returns>Модель данных для удаления документа или null, если документ не найден.</returns>
    Task<DocumentForDeleteModel?> GetDocumentForDeleteAsync(int id);

    /// <summary>
    /// Создает новый документ.
    /// </summary>
    /// <param name="document">Объект документа для создания.</param>
    /// <exception cref="UnauthorizedAccessException">Выбрасывается, если пользователь не аутентифицирован.</exception>
    Task CreateAsync(Document document);

    /// <summary>
    /// Редактирует существующий документ.
    /// </summary>
    /// <param name="document">Объект документа с обновленными данными.</param>
    Task EditAsync(Document document);

    /// <summary>
    /// Удаляет документ.
    /// </summary>
    /// <param name="id">Идентификатор документа для удаления.</param>
    /// <remarks>
    /// Для администраторов выполняется жесткое удаление, для обычных пользователей - мягкое удаление.
    /// </remarks>
    /// <exception cref="UnauthorizedAccessException">Выбрасывается, если пользователь не аутентифицирован.</exception>
    Task DeleteAsync(int id);

    /// <summary>
    /// Восстанавливает удаленный документ.
    /// </summary>
    /// <param name="id">Идентификатор документа для восстановления.</param>
    /// <exception cref="InvalidOperationException">Выбрасывается, если идентификатор сотрудника, удалившего документ, не найден.</exception>
    Task RecoverDeletedAsync(int id);

    /// <summary>
    /// Изменяет статус завершения документа.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    /// <exception cref="NotFoundException">Выбрасывается, если документ не найден.</exception>
    /// <exception cref="IncompleteOutputDocumentException">Выбрасывается, если выходные данные документа не заполнены.</exception>
    Task ChangeStatusAsync(int id);

    /// <summary>
    /// Создает CSV-файл с данными документа.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    /// <returns>Массив байтов, представляющий CSV-файл.</returns>
    /// <exception cref="NotFoundException">Выбрасывается, если документ не найден.</exception>
    Task<byte[]> CreateDocumentCsvAsync(int id);
}