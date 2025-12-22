using TaskManager.Application.Common;
using TaskManager.Application.Dtos.Documents;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Model.Documents;

namespace TaskManager.Application.Services.Interfaces;

/// <summary>
/// Сервис для управления документами.
/// Предоставляет бизнес-логику для операций с документами, включая CRUD-операции,
/// фильтрацию, пагинацию и управление статусами.
/// </summary>
public interface IDocumentService
{
    /// <summary>
    /// Получает постраничный список документов с применением фильтрации.
    /// </summary>
    /// <param name="documentFilterDto">Модель фильтрации документов.</param>
    /// <param name="page">Номер страницы (начинается с 1).</param>
    /// <param name="pageSize">Количество документов на одной странице.</param>
    /// <returns>
    /// Задача, результат которой содержит <see cref="PagedResult{DocumentForOverviewModel}"/>
    /// с отфильтрованными документами и метаданными пагинации.
    /// </returns>
    Task<PagedResult<DocumentForOverviewModel>> GetPagedAsync(
        DocumentFilterDto documentFilterDto,
        int page,
        int pageSize);

    /// <summary>
    /// Получает данные документа для редактирования.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    /// <returns>
    /// Задача, результат которой содержит модель <see cref="DocumentForOverviewEditModel"/>
    /// или <c>null</c>, если документ не найден.
    /// </returns>
    Task<DocumentForOverviewEditModel?> GetDocumentForEditAsync(int id);

    /// <summary>
    /// Получает данные документа для подтверждения удаления.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    /// <returns>
    /// Задача, результат которой содержит модель <see cref="DocumentForDeleteModel"/>
    /// или <c>null</c>, если документ не найден.
    /// </returns>
    Task<DocumentForDeleteModel?> GetDocumentForDeleteAsync(int id);

    /// <summary>
    /// Изменяет статус завершения документа (закрывает / открывает).
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    /// <returns>Задача, представляющая асинхронную операцию изменения статуса.</returns>
    Task ChangeStatusAsync(int id);

    /// <summary>
    /// Создает новый документ.
    /// </summary>
    /// <param name="createdDocumentDto">Документ для создания.</param>
    /// <returns>Задача, представляющая асинхронную операцию создания.</returns>
    Task CreateAsync(CreatedDocumentDto createdDocumentDto);

    /// <summary>
    /// Редактирует существующий документ.
    /// </summary>
    /// <param name="editedDocumentDto">Документ с обновленными данными.</param>
    /// <returns>Задача, представляющая асинхронную операцию редактирования.</returns>
    Task EditAsync(EditedDocumentDto editedDocumentDto);

    /// <summary>
    /// Восстанавливает удаленный документ (отменяет мягкое удаление).
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    /// <returns>Задача, представляющая асинхронную операцию восстановления.</returns>
    /// <remarks>
    /// Метод может использоваться только для восстановления документов,
    /// удаленных с помощью мягкого удаления.
    /// </remarks>
    Task RecoverDeletedAsync(int id);

    /// <summary>
    /// Удаляет документ с учетом прав доступа пользователя.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    /// <returns>Задача, представляющая асинхронную операцию удаления.</returns>
    /// <exception cref="UnauthorizedAccessException">
    /// Выбрасывается, если пользователь не аутентифицирован.
    /// </exception>
    Task DeleteAsync(int id);

    /// <summary>
    /// Создает CSV-файл с данными документа для экспорта.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    /// <returns>
    /// Задача, результат которой содержит массив байтов для CSV-файла.
    /// </returns>
    Task<byte[]> CreateDocumentCsvAsync(int id);
}