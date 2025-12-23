namespace TaskManager.Application.Common;

/// <summary>
/// Представляет результат постраничного разделения данных, содержащий элементы страницы и метаданные пагинации.
/// </summary>
/// <typeparam name="T">Тип элементов в коллекции.</typeparam>
/// <param name="Items">Перечисление элементов на текущей странице.</param>
/// <param name="TotalCount">Общее количество элементов во всех страницах.</param>
/// <param name="PageNumber">Номер текущей страницы (начинается с 1).</param>
/// <param name="PageSize">Количество элементов на одной странице.</param>
public sealed record PagedResult<T>(
    IEnumerable<T> Items,
    int TotalCount,
    int PageNumber,
    int PageSize)
{
    /// <summary>
    /// Получает общее количество страниц.
    /// </summary>
    /// <value>
    /// Общее количество страниц, вычисляемое как округление вверх от TotalCount / PageSize.
    /// </value>
    /// <example>
    /// Для TotalCount = 25 и PageSize = 10, TotalPages = 3.
    /// </example>
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    /// <summary>
    /// Получает значение, указывающее наличие предыдущей страницы.
    /// </summary>
    /// <value>
    /// true если текущая страница не первая (PageNumber > 1); иначе false.
    /// </value>
    public bool HasPreviousPage => PageNumber > 1;

    /// <summary>
    /// Получает значение, указывающее наличие следующей страницы.
    /// </summary>
    /// <value>
    /// true если текущая страница не последняя (PageNumber < TotalPages); иначе false.
    /// </value>
    public bool HasNextPage => PageNumber < TotalPages;
}