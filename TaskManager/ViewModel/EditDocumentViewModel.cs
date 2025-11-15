using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;

namespace TaskManager.View.ViewModel;

public class EditDocumentViewModel
{
    public required Document Document { get; init; }
    public required SelectList Employees { get; set; }
}