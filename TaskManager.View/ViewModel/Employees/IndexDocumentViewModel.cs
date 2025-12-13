using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManager.Application.Common;
using TaskManager.Domain.Model.Documents;

namespace TaskManager.View.ViewModel.Employees;

public class IndexDocumentViewModel
{
    public required string InputString { get; init; }
    public required bool ShowMyTasks { get; init; }
    public required SelectList CountsDocumentsOnPage { get; init; }
    public required DateOnly? StartOutgoingDocumentDateOutputDocument { get; init; }
    public required DateOnly? EndOutgoingDocumentDateOutputDocument { get; init; }
    public required PagedResult<DocumentForOverviewModel> PagedDocuments { get; init; }
}