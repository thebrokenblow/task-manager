using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces.Queries;
using TaskManager.Domain.Interfaces.Repositories;
using TaskManager.Domain.Interfaces.Services;
using TaskManager.Domain.QueryModels;
using TaskManager.View.Filters;
using TaskManager.View.Services;
using TaskManager.View.ViewModel;

namespace TaskManager.View.Controllers;

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
        List<FilteredRangeDocumentModel> documents;

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

        var employeesForSelect = employees.Select(employee => new SelectedEmployeeModel
        {
            Id = employee.Id,
            FullNameAndDepartment = $"{employee.FullName} ({employee.Department})"
        });

        var selectListEmployees = new SelectList(
                                        employeesForSelect,
                                        nameof(SelectedEmployeeModel.Id),
                                        nameof(SelectedEmployeeModel.FullNameAndDepartment));

        ViewData[nameof(SelectedEmployeeModel)] = selectListEmployees;

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

        var employeesForSelect = employees.Select(employee => new SelectedEmployeeModel
        {
            Id = employee.Id,
            FullNameAndDepartment = $"{employee.FullName} ({employee.Department})"
        });

        var selectListEmployees = new SelectList(
                                        employeesForSelect,
                                        nameof(SelectedEmployeeModel.Id),
                                        nameof(SelectedEmployeeModel.FullNameAndDepartment));

        ViewData[nameof(SelectedEmployeeModel)] = selectListEmployees;

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