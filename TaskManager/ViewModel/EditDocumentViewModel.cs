using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManager.Models;

namespace TaskManager.ViewModel;

public class EditDocumentViewModel
{
    public required Document Document { get; init; }
    public required SelectList Employees { get; set; }
}