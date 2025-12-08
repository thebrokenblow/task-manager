namespace TaskManager.Domain.Enums;

/// <summary>
/// Перечисление, представляющее статус документа в системе документооборота.
/// </summary>
public enum DocumentStatus
{
    /// <summary>
    /// Документ является активным и находится в рабочем процессе.
    /// </summary>
    Active,

    /// <summary>
    /// Документ архивирован и более не используется в активных процессах.
    /// </summary>
    Archived,
}