using TaskManager.Domain.QueryModels;

namespace TaskManager.View.ViewModel;

public class IndexDocumentViewModel
{
    public required string InputString { get; init; }
    public required PaginationViewModel PaginationViewModel { get; init; }
    public required List<FilteredRangeDocumentModel> Documents { get; init; }
}