using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Exceptions;
using TaskManager.Domain.Repositories;
using TaskManager.Persistence.Data;

namespace TaskManager.Persistence.Repositories;

/// <summary>
/// Репозиторий для работы с документами.
/// </summary>
public class DocumentRepository(TaskManagerDbContext context) : IDocumentRepository
{
    /// <summary>
    /// Получает документ по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    public async Task<Document?> GetByIdAsync(int id)
    {
        var document = await context.Documents.FindAsync(id);

        return document;
    }

    /// <summary>
    /// Добавляет новый документ.
    /// </summary>
    /// <param name="document">Документ для добавления.</param>
    public async Task AddAsync(Document document)
    {
        await context.AddAsync(document);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Обновляет существующий документ.
    /// </summary>
    /// <param name="document">Документ с обновленными данными.</param>
    public async Task UpdateAsync(Document document)
    {
        context.Update(document);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Удаляет документ полностью из базы данных.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    public async Task RemoveHardAsync(int id)
    {
        var affectedRows = await context.Documents
                                        .Where(document => document.Id == id)
                                        .ExecuteDeleteAsync();

        if (affectedRows == 0)
        {
            throw new NotFoundException(nameof(Document), id);
        }
    }

    /// <summary>
    /// Помечает документ как удаленный (мягкое удаление).
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    /// <param name="idEmployeeRemove">Идентификатор сотрудника, удалившего документ.</param>
    /// <param name="idAdmin">Идентификатор администратора.</param>
    /// <param name="removeDateTime">Дата и время удаления.</param>
    public async Task RemoveSoftAsync(int id, int idEmployeeRemove, int idAdmin, DateTime removeDateTime)
    {
        var affectedRows = await context.Documents
            .Where(document => document.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(document => document.RemovedByEmployeeId, idEmployeeRemove)
                .SetProperty(document => document.RemoveDateTime, removeDateTime)
                .SetProperty(document => document.CreatedByEmployeeId, idAdmin)
            );

        if (affectedRows == 0)
        {
            throw new NotFoundException(nameof(Document), id);
        }
    }

    /// <summary>
    /// Восстанавливает удаленный документ.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    /// <param name="idEmployeeRemove">Идентификатор сотрудника, удалившего документ.</param>
    public async Task RecoverDeletedAsync(int id, int idEmployeeRemove)
    {
        var affectedRows = await context.Documents
            .Where(document => document.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(document => document.CreatedByEmployeeId, idEmployeeRemove)
                .SetProperty(document => document.RemovedByEmployeeId, (int?)null)
                .SetProperty(document => document.RemoveDateTime, (DateTime?)null)
            );

        if (affectedRows == 0)
        {
            throw new NotFoundException(nameof(Document), id);
        }
    }

    /// <summary>
    /// Обновляет статус завершения документа.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    /// <param name="isCompleted">Новый статус завершения.</param>
    public async Task UpdateStatusAsync(int id, bool isCompleted)
    {
        var affectedRows = await context.Documents
            .Where(document => document.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(document => document.IsCompleted, isCompleted)
            );

        if (affectedRows == 0)
        {
            throw new NotFoundException(nameof(Document), id);
        }
    }
}