using TaskManager.Domain.Model.Documents.Query;
using TaskManager.Domain.Services;

namespace TaskManager.View.Utilities;

public sealed class DocumentStyler(IAuthService authService)
{
    /// <summary>
    /// Максимальное количество дней, при котором документ считается просроченным.
    /// Значение 0 означает, что документ просрочен, если срок выполнения наступил или уже прошел.
    /// </summary>
    public const int OverdueDay = 0;

    /// <summary>
    /// Максимальное количество дней, при котором документ требует повышенного внимания.
    /// Документы со сроком выполнения ≤ 3 дней выделяются предупреждающим цветом.
    /// </summary>
    public const int WarningDay = 3;

    /// <summary>
    /// Максимальное количество дней, при котором документ считается находящимся в нормальном состоянии.
    /// Документы со сроком выполнения ≤ 7 дней выделяются успешным цветом.
    /// </summary>
    public const int NormalDay = 7;

    /// <summary>
    /// Определяет CSS класс для строки таблицы на основе оставшегося времени до дедлайна документа.
    /// Возвращает цветовой класс в зависимости от близости срока выполнения.
    /// </summary>
    /// <param name="document">Документ для анализа</param>
    /// <returns>CSS класс для оформления строки таблицы</returns>
    public string DetermineRowColor(DocumentForOverviewModel document)
    {
        var currentDate = DateOnly.FromDateTime(DateTime.Now);
        int daysRemaining = document.TaskDueDateInputDocument.DayNumber - currentDate.DayNumber;

        if (daysRemaining <= OverdueDay)
        {
            return TableCssClassDictionary.TableDanger;
        }

        if (daysRemaining <= WarningDay)
        {
            return TableCssClassDictionary.TableWarning;
        }

        if (daysRemaining <= NormalDay)
        {
            return TableCssClassDictionary.TableSuccess;
        }

        return string.Empty;
    }

    /// <summary>
    /// Получает CSS класс для выделения документов, требующих особого внимания.
    /// </summary>
    /// <param name="document">Документ для проверки</param>
    /// <returns>CSS класс для выделения или пустая строка</returns>
    public string GetControlHighlight(DocumentForOverviewModel document)
    {
        if (document.IsUnderControl)
        {
            return CssClassDictionary.FontWeightBold;
        }

        return string.Empty;
    }

    /// <summary>
    /// Определяет, является ли документ удаленным в системе.
    /// Документ считается удаленным, если он был создан администратором и имеет информацию об удалении
    /// (идентификатор удалившего сотрудника и дата/время удаления).
    /// </summary>
    /// <param name="document">Модель документа для проверки</param>
    /// <returns>
    /// true - если документ был удален администратором;
    /// false - если документ активен или был удален не администратором
    /// </returns>
    /// <remarks>
    /// Логика проверки:
    /// 1. Документ должен быть создан администратором (CreatedByEmployeeId == IdAdmin)
    /// 2. Должен быть указан сотрудник, удаливший документ (RemovedByEmployeeId != null)
    /// 3. Должна быть указана дата/время удаления (RemoveDateTime != null)
    /// </remarks>
    public bool IsDeletedDocument(DocumentForOverviewModel document)
    {
        return document.CreatedByEmployeeId == authService.IdAdmin &&
               document.RemovedByEmployeeId is not null &&
               document.RemoveDateTime is not null;
    }
}