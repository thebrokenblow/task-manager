namespace TaskManager.Application.Utilities;

/// <summary>
/// Предоставляет методы для обработки строк документов.
/// </summary>
public sealed class DocumentStringProcessor
{
    private const string SpaceSeparator = " ";
    private const string UnderscoreSeparator = "_";
    private const string TabSeparator = "\t";
    private const string NewLineSeparator = "\n";
    private const string CarriageReturnSeparator = "\r";
    private const string WindowsNewLineSeparator = "\r\n";
    private const string IdMarker = "id";
    private const string IdPrefix = "id_";

    private static readonly string[] Separators =
    [
        WindowsNewLineSeparator,
        SpaceSeparator,
        UnderscoreSeparator,
        TabSeparator,
        NewLineSeparator,
        CarriageReturnSeparator
    ];

    /// <summary>
    /// Обрабатывает строку документа: удаляет "id" (без учета регистра), 
    /// разбивает на элементы и соединяет их через нижнее подчеркивание,
    /// добавляя префикс "id_" в начале. Если после удаления "id" не остается элементов,
    /// возвращает пустую строку.
    /// </summary>
    /// <param name="input">Входная строки для обработки. Может быть null или пустой.</param>
    /// <returns>
    /// Обработанная строка с префиксом "id_", 
    /// null если входное значение null,
    /// string.Empty если входная строка пустая, содержит только пробелы
    /// или состоит только из "id" в различных регистрах.
    /// </returns>
    public static string? ProcessDocumentSubject(string? input)
    {
        if (input is null)
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        var lowerInput = input.ToLowerInvariant();
        var elements = lowerInput
            .Split(Separators, StringSplitOptions.RemoveEmptyEntries)
            .Where(element => element != IdMarker)
            .ToArray();

        if (elements.Length == 0)
        {
            return string.Empty;
        }

        return $"{IdPrefix}{string.Join(UnderscoreSeparator, elements)}";
    }
}