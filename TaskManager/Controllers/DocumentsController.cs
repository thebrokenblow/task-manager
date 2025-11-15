using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManager.Filters;
using TaskManager.Models;
using TaskManager.Queries.Interfaces;
using TaskManager.Repositories.Interfaces;
using TaskManager.Services;
using TaskManager.Services.Interfaces;
using TaskManager.ViewModel;

namespace TaskManager.Controllers;

public class DocumentsController(
        IAuthService authService,
        IDocumentRepository documentRepository,
        IDocumentQuery documentQuery,
        IEmployeeRepository employeeRepository) : Controller
{
    private const int defaultNumberPage = 1;
    private const int defaultCountDocumentsOnPage = 50;

    [HttpGet]
    public async Task<IActionResult> Index(
        string inputSearch,
        int page = defaultNumberPage,
        int pageSize = defaultCountDocumentsOnPage) 
    {
        int countDocuments;
        List<FilteredRangeDocument> documents;

        if (authService.IsAdmin)
        {
            (documents, countDocuments) = await documentQuery.GetDeletedRangeAsync(inputSearch, (page - 1) * pageSize, pageSize);
        }
        else if (!string.IsNullOrWhiteSpace(inputSearch))
        {
            (documents, countDocuments) = await documentQuery.GetFilteredRangeAsync(inputSearch, (page - 1) * pageSize, pageSize);
        }
        else
        {
            (documents, countDocuments) = await documentQuery.GetRangeAsync((page - 1) * pageSize, pageSize);
        }

        var paginationViewModel = new PaginationViewModel
        {
            CurrentPage = page,
            PageSize = pageSize,
            TotalCount = countDocuments,
            TotalPages = (int)Math.Ceiling(countDocuments / (double)pageSize)
        };

        var indexDocumentViewModel = new IndexDocumentViewModel
        {
            Documents = documents,
            InputString = inputSearch,
            PaginationViewModel = paginationViewModel
        };

        return View(indexDocumentViewModel);
    }

    [HttpGet]
    [AuthenticatedUser]
    public async Task<IActionResult> Create()
    {
        var employees = await employeeRepository.GetAllAsync();

        var employeesForSelect = employees.Select(employee => new EmployeeForSelect
        {
            Id = employee.Id,
            FullNameAndDepartment = $"{employee.FullName} ({employee.Department})"
        });

        var selectListEmployees = new SelectList(
                                        employeesForSelect,
                                        nameof(EmployeeForSelect.Id),
                                        nameof(EmployeeForSelect.FullNameAndDepartment));

        ViewData[nameof(EmployeeForSelect)] = selectListEmployees;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Document document)
    {
        if (authService.CurrentUserId is null)
        {
            return RedirectToAction("Index", "Accounts");
        }

        await documentRepository.AddAsync(document);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var document = await documentRepository.GetByIdAsync(id);

        if (document == null)
        {
            return NotFound();
        }

        var employees = await employeeRepository.GetAllAsync();

        var employeesForSelect = employees.Select(employee => new EmployeeForSelect
        {
            Id = employee.Id,
            FullNameAndDepartment = $"{employee.FullName} ({employee.Department})"
        });

        var selectListEmployees = new SelectList(
                                        employeesForSelect,
                                        nameof(EmployeeForSelect.Id),
                                        nameof(EmployeeForSelect.FullNameAndDepartment));

        ViewData[nameof(EmployeeForSelect)] = selectListEmployees;

        return View(document);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Document document)
    {
        await documentRepository.UpdateAsync(document);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [DocumentOwnerAuthorization]
    public async Task<IActionResult> Delete(int id)
    {
        var document = await documentQuery.GetDetailsByIdAsync(id);

        if (document == null)
        {
            return NotFound();
        }

        return View(document);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var document = await documentRepository.GetByIdAsync(id);

        if (document == null)
        {
            return NotFound();
        }

        if (authService.IsAdmin)
        {
            await documentRepository.RemoveAsync(document);
        }
        else if (authService.IsAuthenticated)
        {
            await documentRepository.ChangeAuthorAsync(document, AuthService.IdAdmin);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> RecoverDeletedTask(int id)
    {
        var document = await documentQuery.GetDetailsByIdAsync(id);

        if (document == null)
        {
            return NotFound();
        }

        await documentRepository.RecoverDeletedTaskAsync(document);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> ChangeStatusTask(int id)
    {
        var document = await documentRepository.GetByIdAsync(id);

        if (document == null)
        {
            return NotFound();
        }

        document.IsCompleted = !document.IsCompleted;
        await documentRepository.UpdateAsync(document);

        return RedirectToAction(nameof(Index));
    }
}