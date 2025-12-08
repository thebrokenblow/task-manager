using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Repositories;

/// <summary>
/// Интерфейс для выполнения операций с документами в системе.
/// Определяет контракты для работы с хранилищем данных (базой данных).
/// </summary>
public interface IDocumentRepository
{
    /// <summary>
    /// Получает документ по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор документа</param>
    /// <returns>Документ или null, если документ не найден</returns>
    Task<Document?> GetByIdAsync(int id);

    /// <summary>
    /// Добавляет новый документ в хранилище.
    /// </summary>
    /// <param name="document">Документ для добавления</param>
    Task AddAsync(Document document);

    /// <summary>
    /// Обновляет существующий документ в хранилище.
    /// </summary>
    /// <param name="document">Документ с обновленными данными</param>
    Task UpdateAsync(Document document);

    /// <summary>
    /// Выполняет полное удаление документа из хранилища.
    /// </summary>
    /// <param name="id">Идентификатор документа для удаления</param>
    Task RemoveHardAsync(int id);

    /// <summary>
    /// Выполняет мягкое удаление документа с сохранением в архиве.
    /// </summary>
    /// <param name="id">Идентификатор документа</param>
    /// <param name="idEmployeeRemove">Идентификатор сотрудника, выполняющего удаление</param>
    /// <param name="idAdmin">Идентификатор администратора</param>
    /// <param name="removeDateTime">Дата и время удаления</param>
    Task RemoveSoftAsync(int id, int idEmployeeRemove, int idAdmin, DateTime removeDateTime);

    /// <summary>
    /// Восстанавливает ранее удаленный документ.
    /// </summary>
    /// <param name="id">Идентификатор документа</param>
    /// <param name="idEmployeeRemove">Идентификатор сотрудника, который изначально создал документ</param>
    Task RecoverDeletedAsync(int id, int idEmployeeRemove);

    /// <summary>
    /// Обновляет статус завершения документа.
    /// </summary>
    /// <param name="id">Идентификатор документа</param>
    /// <param name="isCompleted">Новый статус завершения</param>
    Task UpdateStatusAsync(int id, bool isCompleted);
}