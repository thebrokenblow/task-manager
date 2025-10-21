using TaskManager.Models;

namespace TaskManager.ViewModel;

public class IndexDocumentViewModel
{
    public required List<FilteredRangeDocument> Documents { get; init; }
    public required string InputString { get; init; }
    public required PaginationViewModel PaginationViewModel { get; init; }
}