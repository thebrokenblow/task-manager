using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Mime;
using TaskManager.Application.Exceptions;
using TaskManager.Application.Services.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Exceptions;
using TaskManager.View.Filters;
using TaskManager.View.Utils;
using TaskManager.View.ViewModel.Documents;
using TaskManager.View.ViewModel.Employees;

namespace TaskManager.View.Controllers;

public class DocumentsController(
    IDocumentService documentService,
    IEmployeeService employeeService) : Controller
{
    private const int DefaultNumberPage = 1;
    private const int DefaultCountDocumentsOnPage = 50;

    private const int DefaultDueDateDaysOffset = 5;

    private readonly int[] CountsDocumentsOnPage = [DefaultCountDocumentsOnPage, 75, 150, 200];

    [HttpGet]
    public async Task<IActionResult> Index(
        string inputSearch,
        int totalPages,
        DateOnly? startOutgoingDocumentDateOutputDocument,
        DateOnly? endOutgoingDocumentDateOutputDocument,
        bool showMyTasks,
        int page = DefaultNumberPage,
        int pageSize = DefaultCountDocumentsOnPage) 
    {
        try
        {
            if (!CountsDocumentsOnPage.Contains(pageSize))
            {
                pageSize = DefaultCountDocumentsOnPage;
            }

            if (page < 0 || page > totalPages)
            {
                page = DefaultNumberPage;
            }

            var pagedDocuments = await documentService.GetPagedAsync(
                inputSearch,
                showMyTasks,
                startOutgoingDocumentDateOutputDocument,
                endOutgoingDocumentDateOutputDocument,
                page,
                pageSize);

            var indexDocumentViewModel = new IndexDocumentViewModel
            {
                InputString = inputSearch,
                PagedDocuments = pagedDocuments,
                CountsDocumentsOnPage = new(CountsDocumentsOnPage),
                ShowMyTasks = showMyTasks,
                StartOutgoingDocumentDateOutputDocument = startOutgoingDocumentDateOutputDocument,
                EndOutgoingDocumentDateOutputDocument = endOutgoingDocumentDateOutputDocument,
            };

            return View(indexDocumentViewModel);
        }
        catch (UnauthorizedAccessException)
        {
            return RedirectToUnauthorizedError();
        }
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var document = new Document
        {
            IsExternalDocumentInputDocument = true,
            IncomingDocumentNumberInputDocument = string.Empty,
            IncomingDocumentDateInputDocument = DateOnly.FromDateTime(DateTime.Today),
            TaskDueDateInputDocument = DateOnly.FromDateTime(DateTime.Today.AddDays(DefaultDueDateDaysOffset)),
            IsExternalDocumentOutputDocument = true,
            IsUnderControl = false,
            IsCompleted = false,
            CreatedByEmployeeId = default,
        };

        var responsibleEmployees = await employeeService.GetResponsibleEmployeesAsync();

        var responsibleEmployeesSelectList = new SelectList(
                                                    responsibleEmployees,
                                                    nameof(EmployeeForSelect.Id),
                                                    nameof(EmployeeForSelect.FullNameAndDepartment));

        var createDocumentViewModel = new CreateDocumentViewModel
        {
            Document = document,
            ResponsibleEmployees = responsibleEmployeesSelectList,
        };

        return View(createDocumentViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Document document)
    {
        try
        {
            await documentService.CreateAsync(document);
            return RedirectToAction(nameof(Index));
        }
        catch (UnauthorizedAccessException)
        {
            return RedirectToUnauthorizedError();
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, string? errorMessage = null)
    {
        var document = await documentService.GetDocumentForEditAsync(id);

        if (document is null)
        {
            return RedirectToNotFoundError();
        }

        var responsibleEmployees = await employeeService.GetResponsibleEmployeesAsync();


        var responsibleEmployeesSelectList = new SelectList(
                                                    responsibleEmployees,
                                                    nameof(EmployeeForSelect.Id),
                                                    nameof(EmployeeForSelect.FullNameAndDepartment));

        var editDocumentViewModel = new EditDocumentViewModel
        {
            Document = document,
            ResponsibleEmployees = responsibleEmployeesSelectList,
            ErrorMessage = errorMessage
        };

        return View(editDocumentViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Document document)
    {
        await documentService.EditAsync(document);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [OwnerDocumentOrAdmin]
    public async Task<IActionResult> Delete(int id)
    {
        var document = await documentService.GetDocumentForDeleteAsync(id);

        if (document == null)
        {
            return RedirectToNotFoundError();
        }

        return View(document);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await documentService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch (NotFoundException)
        {
            return RedirectToNotFoundError();
        }
    }

    [HttpPost]
    public async Task<IActionResult> RecoverDeleted(int id)
    {
        try
        {
            await documentService.RecoverDeletedAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch (NotFoundException)
        {
            return RedirectToNotFoundError();
        }
    }

    [HttpPost]
    public async Task<IActionResult> ChangeStatus(int id)
    {
        try
        {
            await documentService.ChangeStatusAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch (NotFoundException)
        {
            return RedirectToNotFoundError();
        }
        catch (IncompleteOutputDocumentException exception)
        {
            var nameAction = nameof(Edit);
            var fullNameController = nameof(DocumentsController);
            var nameController = NameController.GetControllerName(fullNameController);

            return RedirectToAction(nameAction, nameController, new { id, errorMessage = exception.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> ExportToCsv(int id)
    {
        try
        {
            var bytesDocument = await documentService.CreateDocumentCsvAsync(id);
            return File(bytesDocument, MediaTypeNames.Text.Csv, "document.csv");
        }
        catch (NotFoundException)
        {
            return RedirectToNotFoundError();
        }
    }

    private RedirectToActionResult RedirectToUnauthorizedError()
    {
        var nameAction = nameof(AccountsController.Login);
        var nameController = NameController.GetControllerName(nameof(AccountsController));

        return RedirectToAction(nameAction, nameController);
    }

    private RedirectToActionResult RedirectToNotFoundError()
    {
        var nameAction = nameof(ErrorsController.DocumentNotFoundError);
        var nameController = NameController.GetControllerName(nameof(ErrorsController));

        return RedirectToAction(nameAction, nameController);
    }
}