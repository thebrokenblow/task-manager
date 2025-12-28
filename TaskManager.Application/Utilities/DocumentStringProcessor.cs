namespace TaskManager.Application.Utilities;

/// <summary>
/// Предоставляет методы для обработки строк документов.
/// </summary>
public sealed class DocumentStringProcessor
{
    private const string SpaceSeparator = " ";
    private const string UnderscoreSeparator = "_";
    private const string IdMarker = "id";
    private const string IdPrefix = "id_";

    /// <summary>
    /// Обрабатывает строку документа: удаляет "id" (без учета регистра), 
    /// разбивает на элементы и соединяет их через нижнее подчеркивание,
    /// добавляя префикс "id_" в начале. Если после удаления "id" не остается элементов,
    /// возвращает пустую строку.
    /// </summary>
    /// <param name="input">Входная строка для обработки. Может быть null или пустой.</param>
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

        var withoutId = input.ToLowerInvariant().Replace(IdMarker, string.Empty);
        var elements = withoutId.Split(SpaceSeparator, StringSplitOptions.RemoveEmptyEntries);

        if (elements.Length == 0)
        {
            return string.Empty;
        }

        return $"{IdPrefix}{string.Join(UnderscoreSeparator, elements)}";
    }
}