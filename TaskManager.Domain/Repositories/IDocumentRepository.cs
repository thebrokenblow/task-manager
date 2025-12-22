using TaskManager.Domain.Entities;
using TaskManager.Domain.Model.Documents.Edit;

namespace TaskManager.Domain.Repositories;

/// <summary>
/// Репозиторий для работы с сущностью <see cref="Document"/>.
/// Предоставляет методы для доступа и управления данными документов.
/// </summary>
public interface IDocumentRepository
{
    /// <summary>
    /// Добавляет новый документ в систему.
    /// </summary>
    /// <param name="document">Документ для добавления.</param>
    /// <returns>Задача, представляющая асинхронную операцию добавления.</returns>
    Task AddAsync(Document document);

    /// <summary>
    /// Обновляет данные существующего документа.
    /// </summary>
    /// <param name="documentForEditModel">Документ с обновленными данными.</param>
    /// <returns>Задача, представляющая асинхронную операцию обновления.</returns>
    Task UpdateAsync(DocumentForEditModel documentForEditModel);

    /// <summary>
    /// Полностью удаляет документ из базы данных (жесткое удаление).
    /// </summary>
    /// <param name="id">Идентификатор документа для удаления.</param>
    /// <returns>Задача, представляющая асинхронную операцию удаления.</returns>
    Task RemoveHardAsync(int id);

    /// <summary>
    /// Помечает документ как удаленный (мягкое удаление).
    /// Устанавливает информацию об удалении без физического удаления записи из базы данных.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    /// <param name="idEmployeeRemove">Идентификатор сотрудника, выполняющего удаление.</param>
    /// <param name="idAdmin">Идентификатор администратора, утверждающего удаление.</param>
    /// <param name="removeDateTime">Дата и время удаления.</param>
    /// <returns>Задача, представляющая асинхронную операцию.</returns>
    Task RemoveSoftAsync(int id, int idEmployeeRemove, int idAdmin, DateTime removeDateTime);

    /// <summary>
    /// Восстанавливает удаленный документ (отменяет мягкое удаление).
    /// </summary>
    /// <param name="id">Идентификатор документа для восстановления.</param>
    /// <param name="idEmployeeRemove">Идентификатор сотрудника, выполняющего восстановление.</param>
    /// <returns>Задача, представляющая асинхронную операцию.</returns>
    Task RecoverDeletedAsync(int id, int idEmployeeRemove);

    /// <summary>
    /// Обновляет статус завершения документа.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    /// <param name="isCompleted">Новый статус завершения.</param>
    /// <returns>Задача, представляющая асинхронную операцию.</returns>
    Task UpdateStatusAsync(int id, bool isCompleted);
}