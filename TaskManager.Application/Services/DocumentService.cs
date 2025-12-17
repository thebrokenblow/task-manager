using TaskManager.Application.Common;
using TaskManager.Application.Dtos.Documents;
using TaskManager.Application.Exceptions;
using TaskManager.Application.Services.Interfaces;
using TaskManager.Application.Validations;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Exceptions;
using TaskManager.Domain.Model.Departments;
using TaskManager.Domain.Model.Documents;
using TaskManager.Domain.Queries;
using TaskManager.Domain.Repositories;
using TaskManager.Domain.Services;

namespace TaskManager.Application.Services;

/// <summary>
/// Сервис для управления документами.
/// Предоставляет бизнес-логику для операций с документами, включая CRUD-операции,
/// фильтрацию, пагинацию и управление статусами.
/// </summary>
public class DocumentService(
    IDepartmentQuery departmentQuery,
    IDocumentQuery documentQuery,
    IDocumentRepository documentRepository,
    IAuthService authService,
    IExportService exportService) : IDocumentService
{
    private readonly IDepartmentQuery _departmentQuery = 
        departmentQuery ?? throw new ArgumentNullException(nameof(departmentQuery));

    private readonly IDocumentQuery _documentQuery = 
        documentQuery ?? throw new ArgumentNullException(nameof(documentQuery));

    private readonly IDocumentRepository _documentRepository = 
        documentRepository ?? throw new ArgumentNullException(nameof(documentRepository));

    private readonly IAuthService _authService = 
        authService ?? throw new ArgumentNullException(nameof(authService));

    private readonly IExportService _exportService = 
        exportService ?? throw new ArgumentNullException(nameof(exportService));

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
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если <paramref name="documentFilterDto"/> равен <c>null</c>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Выбрасывается, если <paramref name="page"/> меньше 1 или <paramref name="pageSize"/> меньше или равно 0.
    /// </exception>
    /// <exception cref="UnauthorizedAccessException">
    /// Выбрасывается, если пользователь не аутентифицирован при попытке получения "мои документы".
    /// </exception>
    /// <remarks>
    /// <para>Процесс фильтрации:</para>
    /// <para>1. Определяется отдел текущего пользователя (если аутентифицирован)</para>
    /// <para>2. Если установлен флаг <see cref="DocumentFilterDto.IsShowMyDocuments"/>, 
    /// фильтруются только документы текущего пользователя</para>
    /// <para>3. Применяются фильтры по датам и поисковому запросу</para>
    /// <para>4. Документы сортируются по дате выполнения задачи и флагу контроля</para>
    /// </remarks>
    public async Task<PagedResult<DocumentForOverviewModel>> GetPagedAsync(
        DocumentFilterDto documentFilterDto,
        int page,
        int pageSize)
    {
        ArgumentNullException.ThrowIfNull(documentFilterDto);

        if (page < 1)
        {
            throw new ArgumentException("Номер страницы должен быть больше или равен 1", nameof(page));
        }

        if (pageSize <= 0)
        {
            throw new ArgumentException("Размер страницы должен быть больше 0", nameof(pageSize));
        }

        // Определение фильтрации по сотруднику ("мои документы")
        int? idResponsibleEmployeeInputDocument = null;

        if (documentFilterDto.IsShowMyDocuments)
        {
            if (!_authService.IsAuthenticated || !_authService.IdCurrentUser.HasValue)
            {
                throw new UnauthorizedAccessException("Пользователь не аутентифицирован для просмотра своих документов");
            }

            idResponsibleEmployeeInputDocument = _authService.IdCurrentUser;
        }

        // Получение отдела текущего пользователя для фильтрации по отделу
        DepartmentModel? departmentModel = null;

        if (_authService.IsAuthenticated && _authService.IdCurrentUser.HasValue)
        {
            departmentModel = await _departmentQuery.GetDepartmentByEmployeeIdAsync(_authService.IdCurrentUser.Value);
        }

        var documentFilterModel = new DocumentFilterModel
        {
            SearchTerm = documentFilterDto.SearchTerm,
            StartOutgoingDocumentDateOutputDocument = documentFilterDto.StartOutgoingDocumentDateOutputDocument,
            EndOutgoingDocumentDateOutputDocument = documentFilterDto.EndOutgoingDocumentDateOutputDocument,
            IdResponsibleEmployeeInputDocument = idResponsibleEmployeeInputDocument,
            ResponsibleDepartmentInputDocument = departmentModel?.Name,
            UserRole = _authService.Role,
        };

        // Вычисление параметров пагинации
        int countSkip = (page - 1) * pageSize;

        var (documents, countDocuments) = await _documentQuery.GetDocumentsAsync(
            documentFilterModel,
            countSkip,
            pageSize);

        var pagedResult = new PagedResult<DocumentForOverviewModel>(
            documents,
            countDocuments,
            page,
            pageSize);

        return pagedResult;
    }

    /// <summary>
    /// Получает данные документа для редактирования.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    /// <returns>
    /// Задача, результат которой содержит модель <see cref="DocumentForEditModel"/>
    /// или <c>null</c>, если документ не найден.
    /// </returns>
    public async Task<DocumentForEditModel?> GetDocumentForEditAsync(int id)
    {
        var document = await _documentQuery.GetDocumentForEditAsync(id);

        return document;
    }

    /// <summary>
    /// Получает данные документа для подтверждения удаления.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    /// <returns>
    /// Задача, результат которой содержит модель <see cref="DocumentForDeleteModel"/>
    /// или <c>null</c>, если документ не найден.
    /// </returns>
    public async Task<DocumentForDeleteModel?> GetDocumentForDeleteAsync(int id)
    {
        var document = await _documentQuery.GetDocumentForDeleteAsync(id);

        return document;
    }

    /// <summary>
    /// Изменяет статус завершения документа (закрывает / открывает).
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    /// <returns>Задача, представляющая асинхронную операцию изменения статуса.</returns>
    /// <exception cref="NotFoundException">
    /// Выбрасывается, если документ с указанным <paramref name="id"/> не найден.
    /// </exception>
    /// <exception cref="IncompleteOutputDocumentException">
    /// Выбрасывается, если документ не содержит обязательных данных для завершения.
    /// </exception>
    /// <remarks>
    /// <para>Перед изменением статуса выполняется проверка:</para>
    /// <para>1. Существование документа</para>
    /// <para>2. Заполненность обязательных полей исходящего документа (валидация)</para>
    /// <para>3. Статус меняется на противоположный (закрытый ↔ открытый)</para>
    /// </remarks>
    public async Task ChangeStatusAsync(int id)
    {
        var documentForChangeStatusModel = await _documentQuery.GetDocumentForChangeStatusAsync(id) ??
            throw new NotFoundException(nameof(DocumentForChangeStatusModel), id);

        // Валидация данных для изменения статуса
        var documentForChangeStatusModelValidator = new DocumentForChangeStatusModelValidator();
        var validationResult = documentForChangeStatusModelValidator.Validate(documentForChangeStatusModel);

        if (!validationResult.IsValid)
        {
            throw new IncompleteOutputDocumentException();
        }

        // Изменение статуса на противоположный
        await _documentRepository.UpdateStatusAsync(id, !documentForChangeStatusModel.IsCompleted);
    }

    /// <summary>
    /// Создает новый документ.
    /// </summary>
    /// <param name="document">Документ для создания.</param>
    /// <returns>Задача, представляющая асинхронную операцию создания.</returns>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если <paramref name="document"/> равен <c>null</c>.
    /// </exception>
    /// <exception cref="UnauthorizedAccessException">
    /// Выбрасывается, если пользователь не аутентифицирован.
    /// </exception>
    /// <remarks>
    /// <para>Перед сохранением выполняется:</para>
    /// <para>1. Очистка строковых полей от лишних пробелов</para>
    /// <para>2. Установка идентификатора создателя документа</para>
    /// <para>3. Сохранение в базу данных</para>
    /// </remarks>
    public async Task CreateAsync(Document document)
    {
        ArgumentNullException.ThrowIfNull(document);

        if (!_authService.IsAuthenticated || !_authService.IdCurrentUser.HasValue)
        {
            throw new UnauthorizedAccessException("Пользователь не аутентифицирован");
        }

        // Очистка строковых полей
        TrimDocumentStrings(document);

        // Установка создателя документа
        document.CreatedByEmployeeId = _authService.IdCurrentUser.Value;

        await _documentRepository.AddAsync(document);
    }

    /// <summary>
    /// Редактирует существующий документ.
    /// </summary>
    /// <param name="document">Документ с обновленными данными.</param>
    /// <returns>Задача, представляющая асинхронную операцию редактирования.</returns>
    /// <exception cref="ArgumentNullException">
    /// Выбрасывается, если <paramref name="document"/> равен <c>null</c>.
    /// </exception>
    /// <remarks>
    /// <para>При редактировании выполняется:</para>
    /// <para>1. Очистка строковых полей от лишних пробелов</para>
    /// <para>2. Установка даты и автора последнего редактирования</para>
    /// <para>3. Обновление в базе данных</para>
    /// </remarks>
    public async Task EditAsync(Document document)
    {
        ArgumentNullException.ThrowIfNull(document);

        // Очистка строковых полей
        TrimDocumentStrings(document);

        // Установка информации о редактировании
        document.LastEditedDateTime = DateTime.Now;
        document.LastEditedByEmployeeId = _authService.IdCurrentUser;

        await _documentRepository.UpdateAsync(document);
    }

    /// <summary>
    /// Восстанавливает удаленный документ (отменяет мягкое удаление).
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    /// <returns>Задача, представляющая асинхронную операцию восстановления.</returns>
    /// <exception cref="ArgumentException">
    /// Выбрасывается, если <paramref name="id"/> меньше или равен нулю.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Выбрасывается, если не удалось определить сотрудника, удалившего документ.
    /// </exception>
    /// <remarks>
    /// Метод может использоваться только для восстановления документов,
    /// удаленных с помощью мягкого удаления.
    /// </remarks>
    public async Task RecoverDeletedAsync(int id)
    {
        var removedByEmployeeId = await _documentQuery.GetIdEmployeeRemovedAsync(id) ??
            throw new InvalidOperationException(
                $"Не удалось определить сотрудника, удалившего документ с ID: {id}");

        await _documentRepository.RecoverDeletedAsync(id, removedByEmployeeId);
    }

    /// <summary>
    /// Удаляет документ с учетом прав доступа пользователя.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    /// <returns>Задача, представляющая асинхронную операцию удаления.</returns>
    /// <exception cref="UnauthorizedAccessException">
    /// Выбрасывается, если пользователь не аутентифицирован.
    /// </exception>
    /// <remarks>
    /// <para>Логика удаления зависит от роли пользователя:</para>
    /// <para>- Администраторы: жесткое удаление (физическое удаление из БД)</para>
    /// <para>- Обычные пользователи: мягкое удаление (помечается как удаленный)</para>
    /// <para>Мягкое удаление сохраняет информацию о времени удаления и исполнителе.</para>
    /// </remarks>
    public async Task DeleteAsync(int id)
    {
        if (!_authService.IsAuthenticated || !_authService.IdCurrentUser.HasValue)
        {
            throw new UnauthorizedAccessException("Пользователь не аутентифицирован");
        }

        if (_authService.IsAdmin)
        {
            // Жесткое удаление для администраторов
            await _documentRepository.RemoveHardAsync(id);
        }
        else
        {
            // Мягкое удаление для обычных пользователей
            await _documentRepository.RemoveSoftAsync(
                id,
                _authService.IdCurrentUser.Value,
                _authService.IdAdmin,
                DateTime.Now);
        }
    }

    /// <summary>
    /// Создает CSV-файл с данными документа для экспорта.
    /// </summary>
    /// <param name="id">Идентификатор документа.</param>
    /// <returns>
    /// Задача, результат которой содержит массив байтов для CSV-файла.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Выбрасывается, если документ с указанным <paramref name="id"/> не найден.
    /// </exception>
    /// <remarks>
    /// CSV-файл содержит все основные данные документа в структурированном виде,
    /// пригодном для импорта в электронные таблицы.
    /// </remarks>
    public async Task<byte[]> CreateDocumentCsvAsync(int id)
    {
        var documentForCsvExportModel = await _documentQuery.GetDocumentForCsvExportAsync(id) ??
            throw new NotFoundException(nameof(DocumentForCsvExportModel), id);

        var bytesDocument = _exportService.ConvertDocumentToCsv(documentForCsvExportModel);
        return bytesDocument;
    }

    /// <summary>
    /// Очищает строковые свойства документа от лишних пробелов.
    /// </summary>
    /// <param name="document">Документ для обработки.</param>
    private static void TrimDocumentStrings(Document document)
    {
        if (document.OutgoingDocumentNumberInputDocument is not null)
        {
            document.OutgoingDocumentNumberInputDocument = document.OutgoingDocumentNumberInputDocument.Trim();
        }

        if (document.CustomerInputDocument is not null)
        {
            document.CustomerInputDocument = document.CustomerInputDocument.Trim();
        }

        if (document.DocumentSummaryInputDocument is not null)
        {
            document.DocumentSummaryInputDocument = document.DocumentSummaryInputDocument.Trim();
        }

        if (document.IncomingDocumentNumberInputDocument is not null)
        {
            document.IncomingDocumentNumberInputDocument = document.IncomingDocumentNumberInputDocument.Trim();
        }

        if (document.ResponsibleDepartmentsInputDocument is not null)
        {
            document.ResponsibleDepartmentsInputDocument = document.ResponsibleDepartmentsInputDocument.Trim();
        }

        if (document.OutgoingDocumentNumberOutputDocument is not null)
        {
            document.OutgoingDocumentNumberOutputDocument = document.OutgoingDocumentNumberOutputDocument.Trim();
        }

        if (document.RecipientOutputDocument is not null)
        {
            document.RecipientOutputDocument = document.RecipientOutputDocument.Trim();
        }

        if (document.DocumentSummaryOutputDocument is not null)
        {
            document.DocumentSummaryOutputDocument = document.DocumentSummaryOutputDocument.Trim();
        }
    }
}