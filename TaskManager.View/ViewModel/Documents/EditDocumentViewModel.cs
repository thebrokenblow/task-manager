using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManager.Domain.Model.Documents;

namespace TaskManager.View.ViewModel.Documents;

public class EditDocumentViewModel
{
    public required DocumentForEditModel Document { get; init; }
    public required SelectList ResponsibleEmployees { get; init; }
    public string? ErrorMessage { get; init; }
}