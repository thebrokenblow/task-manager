using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManager.Models;
using TaskManager.Queries;
using TaskManager.Queries.Interfaces;
using TaskManager.Repositories.Interfaces;
using TaskManager.ViewModel;

namespace TaskManager.Controllers;

public class DocumentsController(
        IDocumentRepository documentRepository,
        IDocumentQuery documentQuery,
        IEmployeeRepository employeeRepository) : Controller
{
    private const int defaultNumberPage = 1;
    private const int defaultCountDocumentsOnPage = 10;

    [HttpGet]
    public async Task<IActionResult> Index(
        string inputSearch,
        int page = defaultNumberPage,
        int pageSize = defaultCountDocumentsOnPage) 
    {
        var (documents, countDocuments) = await documentQuery.GetFilteredRangeAsync(inputSearch, (page - 1) * pageSize, pageSize);

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

        await documentRepository.RemoveAsync(document);

        return RedirectToAction(nameof(Index));
    }
}