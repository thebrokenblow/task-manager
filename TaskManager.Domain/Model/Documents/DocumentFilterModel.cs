using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Model.Documents;

/// <summary>
/// Параметры фильтрации документов на уровне домена.
/// </summary>
public class DocumentFilterModel
{
    /// <summary>
    /// Поисковый запрос по документам
    /// </summary>
    public required string? SearchTerm { get; init; }

    /// <summary>
    /// Дата исходящего документа от
    /// </summary>
    public required DateOnly? StartOutgoingDocumentDateOutputDocument { get; init; }

    /// <summary>
    /// Дата исходящего документа до
    /// </summary>
    public required DateOnly? EndOutgoingDocumentDateOutputDocument { get; init; }

    /// <summary>
    /// ID ответственного сотрудника
    /// </summary>
    public required int? IdResponsibleEmployeeInputDocument { get; init; }

    /// <summary>
    /// Роль пользователя
    /// </summary>
    public required UserRole? UserRole { get; init; }

    /// <summary>
    /// Ответственный отдел
    /// </summary>
    public required string? ResponsibleDepartmentInputDocument { get; set; }
}