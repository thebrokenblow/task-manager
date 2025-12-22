using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Exceptions;
using TaskManager.Domain.Model.Documents.Edit;
using TaskManager.Domain.Repositories;
using TaskManager.Persistence.Data;

namespace TaskManager.Persistence.Repositories;

/// <summary>
/// Репозиторий для работы с сущностью <see cref="Document"/>.
/// Предоставляет методы для доступа и управления данными документов.
/// </summary>
public class DocumentRepository(TaskManagerDbContext context) : IDocumentRepository
{
    private readonly TaskManagerDbContext _context =
        context ?? throw new ArgumentNullException(nameof(context));

    /// <summary>
    /// Добавляет новый документ в систему.
    /// </summary>
    /// <param name="document">Документ для добавления.</param>
    /// <returns>Задача, представляющая асинхронную операцию добавления.</returns>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если <paramref name="document"/> равен <c>null</c>.
    /// </exception>
    /// <exception cref="DbUpdateException">
    /// Выбрасывается при возникновении ошибок при сохранении в базу данных.
    /// </exception>
    public async Task AddAsync(Document document)
    {
        ArgumentNullException.ThrowIfNull(document);

        await _context.AddAsync(document);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Обновляет данные существующего документа.
    /// </summary>
    /// <param name="documentForEditModel">Документ с обновленными данными.</param>
    /// <returns>Задача, представляющая асинхронную операцию обновления.</returns>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если <paramref name="documentForEditModel"/> равен <c>null</c>.
    /// </exception>
    /// <exception cref="NotFoundException">
    /// Выбрасывается, если документ с указанным Id не найден.
    /// </exception>
    public async Task UpdateAsync(DocumentForEditModel documentForEditModel)
    {
        ArgumentNullException.ThrowIfNull(documentForEditModel);

        var affectedRows = await _context.Documents
            .Where(document => document.Id == documentForEditModel.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(document => document.OutgoingDocumentNumberInputDocument,
                    documentForEditModel.OutgoingDocumentNumberInputDocument)
                .SetProperty(document => document.SourceDocumentDateInputDocument,
                    documentForEditModel.SourceDocumentDateInputDocument)
                .SetProperty(document => document.CustomerInputDocument,
                    documentForEditModel.CustomerInputDocument)
                .SetProperty(document => document.DocumentSummaryInputDocument,
                    documentForEditModel.DocumentSummaryInputDocument)
                .SetProperty(document => document.IsExternalDocumentInputDocument,
                    documentForEditModel.IsExternalDocumentInputDocument)
                .SetProperty(document => document.IncomingDocumentNumberInputDocument,
                    documentForEditModel.IncomingDocumentNumberInputDocument)
                .SetProperty(document => document.IncomingDocumentDateInputDocument,
                    documentForEditModel.IncomingDocumentDateInputDocument)
                .SetProperty(document => document.ResponsibleDepartmentInputDocument,
                    documentForEditModel.ResponsibleDepartmentInputDocument)
                .SetProperty(document => document.ResponsibleDepartmentsInputDocument,
                    documentForEditModel.ResponsibleDepartmentsInputDocument)
                .SetProperty(document => document.TaskDueDateInputDocument,
                    documentForEditModel.TaskDueDateInputDocument)
                .SetProperty(document => document.IdResponsibleEmployeeInputDocument,
                    documentForEditModel.IdResponsibleEmployeeInputDocument)
                .SetProperty(document => document.IsExternalDocumentOutputDocument,
                    documentForEditModel.IsExternalDocumentOutputDocument)
                .SetProperty(document => document.OutgoingDocumentNumberOutputDocument,
                    documentForEditModel.OutgoingDocumentNumberOutputDocument)
                .SetProperty(document => document.OutgoingDocumentDateOutputDocument,
                    documentForEditModel.OutgoingDocumentDateOutputDocument)
                .SetProperty(document => document.RecipientOutputDocument,
                    documentForEditModel.RecipientOutputDocument)
                .SetProperty(document => document.DocumentSummaryOutputDocument,
                    documentForEditModel.DocumentSummaryOutputDocument)
                .SetProperty(document => document.IsUnderControl,
                    documentForEditModel.IsUnderControl)
                .SetProperty(document => document.IsCompleted,
                    documentForEditModel.IsCompleted)
                .SetProperty(document => document.CreatedByEmployeeId,
                    documentForEditModel.CreatedByEmployeeId)
                .SetProperty(document => document.RemoveDateTime,
                    documentForEditModel.RemoveDateTime)
                .SetProperty(document => document.LastEditedDateTime,
                    documentForEditModel.LastEditedDateTime)
                .SetProperty(document => document.LastEditedByEmployeeId,
                    documentForEditModel.LastEditedByEmployeeId)
            );

        if (affectedRows == 0)
        {
            throw new NotFoundException(nameof(Document), documentForEditModel.Id);
        }
    }

    /// <summary>
    /// Полностью удаляет документ из базы данных (жесткое удаление).
    /// </summary>
    /// <param name="id">Идентификатор документа для удаления.</param>
    /// <returns>Задача, представляющая асинхронную операцию удаления.</returns>
    /// <exception cref="NotFoundException">
    /// Выбрасывается, если документ с указанным <paramref name="id"/> не найден.
    /// </exception>
    /// <exception cref="DbUpdateException">
    /// Выбрасывается при возникновении ошибок при удалении из базы данных.
    /// </exception>
    public async Task RemoveHardAsync(int id)
    {
        var affectedRows = await _context.Documents
            .Where(document => document.Id == id)
            .ExecuteDeleteAsync();

        if (affectedRows == 0)
        {
            throw new NotFoundException(nameof(Document), id);
        }
    }

    /// <summary>
    /// Помечает документ как удаленный (мягкое удаление).
    /// Устанавливает информацию об удалении без физического удаления записи из базы данных.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    /// <param name="idEmployeeRemove">Идентификатор сотрудника, выполняющего удаление.</param>
    /// <param name="idAdmin">Идентификатор администратора, утверждающего удаление.</param>
    /// <param name="removeDateTime">Дата и время удаления.</param>
    /// <returns>Задача, представляющая асинхронную операцию.</returns>
    /// <exception cref="NotFoundException">
    /// Выбрасывается, если документ с указанным <paramref name="id"/> не найден.
    /// </exception>
    /// <exception cref="DbUpdateException">
    /// Выбрасывается при возникновении ошибок при обновлении в базе данных.
    /// </exception>
    public async Task RemoveSoftAsync(int id, int idEmployeeRemove, int idAdmin, DateTime removeDateTime)
    {
        var affectedRows = await _context.Documents
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
    /// Восстанавливает удаленный документ (отменяет мягкое удаление).
    /// </summary>
    /// <param name="id">Идентификатор документа для восстановления.</param>
    /// <param name="idEmployeeRemove">Идентификатор сотрудника, выполняющего восстановление.</param>
    /// <returns>Задача, представляющая асинхронную операцию.</returns>
    /// <exception cref="NotFoundException">
    /// Выбрасывается, если документ с указанным <paramref name="id"/> не найден.
    /// </exception>
    /// <exception cref="DbUpdateException">
    /// Выбрасывается при возникновении ошибок при обновлении в базе данных.
    /// </exception>
    public async Task RecoverDeletedAsync(int id, int idEmployeeRemove)
    {
        var affectedRows = await _context.Documents
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
    /// <returns>Задача, представляющая асинхронную операцию.</returns>
    /// <exception cref="NotFoundException">
    /// Выбрасывается, если документ с указанным <paramref name="id"/> не найден.
    /// </exception>
    /// <exception cref="DbUpdateException">
    /// Выбрасывается при возникновении ошибок при обновлении в базе данных.
    /// </exception>
    public async Task UpdateStatusAsync(int id, bool isCompleted)
    {
        var affectedRows = await _context.Documents
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