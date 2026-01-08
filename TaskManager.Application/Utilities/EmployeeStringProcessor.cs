namespace TaskManager.Application.Utilities;

/// <summary>
/// Предоставляет утилиты для обработки строковых данных, связанных с сотрудниками.
/// Содержит методы для очистки и преобразования строковых значений.
/// </summary>
public sealed class EmployeeStringProcessor
{
    private const string SpaceSeparator = " ";
    private const string UnderscoreSeparator = "_";

    /// <summary>
    /// Очищает строку от лишних пробелов.
    /// Удаляет начальные и конечные пробелы, а также лишние пробелы между словами.
    /// </summary>
    /// <param name="input">Входная строка для обработки. Может быть null или пустой.</param>
    /// <returns>Очищенная строка или пустая строка, если входное значение было null или состояло только из пробелов.</returns>
    public static string CleanSpaces(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        var elements = input.Split(SpaceSeparator, StringSplitOptions.RemoveEmptyEntries);

        return string.Join(SpaceSeparator, elements);
    }

    /// <summary>
    /// Преобразует пробелы в строке в символы подчеркивания.
    /// Удаляет начальные и конечные пробелы, а также лишние пробелы между словами перед преобразованием.
    /// </summary>
    /// <param name="input">Входная строка для обработки. Может быть null или пустой.</param>
    /// <returns>Строка с пробелами, замененными на подчеркивания, или пустая строка, если входное значение было null или состояло только из пробелов.</returns>
    public static string ConvertSpacesToUnderscore(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        var elements = input.Split(SpaceSeparator, StringSplitOptions.RemoveEmptyEntries);

        return string.Join(UnderscoreSeparator, elements);
    }
}