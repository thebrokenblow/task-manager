namespace TaskManager.View.Utilities;

/// <summary>
/// Словарь CSS классов для стилизации таблиц.
/// Содержит константы для классов таблиц Bootstrap.
/// </summary>
public sealed class TableCssClassDictionary
{
    /// <summary>
    /// CSS класс Bootstrap для строки таблицы с индикацией опасного/критического состояния.
    /// Обычно отображается красным цветом фона.
    /// </summary>
    public const string TableDanger = "table-danger";

    /// <summary>
    /// CSS класс Bootstrap для строки таблицы с индикацией предупреждения.
    /// Обычно отображается желтым цветом фона.
    /// </summary>
    public const string TableWarning = "table-warning";

    /// <summary>
    /// CSS класс Bootstrap для строки таблицы с индикацией успешного выполнения.
    /// Обычно отображается зеленым цветом фона.
    /// </summary>
    public const string TableSuccess = "table-success";
}