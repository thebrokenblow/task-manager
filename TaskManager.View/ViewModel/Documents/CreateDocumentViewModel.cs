using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManager.Domain.Entities;

namespace TaskManager.View.ViewModel.Documents;

public class CreateDocumentViewModel
{
    public required Document Document { get; init; }
    public required SelectList ResponsibleEmployees { get; init; }
    public required SelectList ResponsibleDepartments { get; init; }
}
