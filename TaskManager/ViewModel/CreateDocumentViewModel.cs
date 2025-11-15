using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManager.Domain.Entities;

namespace TaskManager.View.ViewModel;

public class CreateDocumentViewModel
{
    public Document? Document { get; init; }
    public required SelectList Employees { get; init; }
}
