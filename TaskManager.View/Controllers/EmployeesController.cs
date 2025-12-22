using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TaskManager.Application.Exceptions;
using TaskManager.Application.Services.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.View.Filters;
using TaskManager.View.Utilities;
using TaskManager.View.ViewModel.Employees;

namespace TaskManager.View.Controllers;

public class EmployeesController(IEmployeeService employeeService) : Controller
{
    private const string FailedCreatedEmployeeKey = "FailedCreatedEmployee";
    private const string TextFailedCreatedEmployeeKey = "TextFailedCreatedEmployee";
    
    public const string TextFailedEditedEmployeeKey = "TextFailedEditedEmployeeKey";

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var employees = await employeeService.GetRegularEmployeesAsync();

            var indexEmployeesViewModel = new IndexEmployeesViewModel
            {
                Employees = employees
            };

            if (TempData.TryGetValue(FailedCreatedEmployeeKey, out var failedEmployeeJson) &&
                TempData.TryGetValue(TextFailedCreatedEmployeeKey, out var textFailed))
            {
                if (failedEmployeeJson is not null)
                {
                    var failedEmployee = JsonSerializer.Deserialize<Employee>((string)failedEmployeeJson);
                    indexEmployeesViewModel.FailedCreatedEmployee = failedEmployee;
                }

                if (textFailed is not null)
                {
                    indexEmployeesViewModel.TextFailedCreatedEmployee = textFailed.ToString();
                }
            }

            TempData.Remove(FailedCreatedEmployeeKey);
            TempData.Remove(TextFailedCreatedEmployeeKey);

            return View(indexEmployeesViewModel);
        }
        catch
        {
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(Employee employee)
    {
        try
        {
            await employeeService.CreateAsync(employee);
            return RedirectToAction(nameof(Index));
        }
        catch (LoginAlreadyExistsException)
        {
            TempData[FailedCreatedEmployeeKey] = JsonSerializer.Serialize(employee);
            TempData[TextFailedCreatedEmployeeKey] = "Сотрудник с таким логином уже существует в системе";

            return RedirectToAction(nameof(Index));
        }
        catch
        {
            TempData[TextFailedCreatedEmployeeKey] = "Произошла ошибка при создании сотрудника";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet]
    [AdminOnly]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var employee = await employeeService.GetEmployeeForEditAsync(id);

            if (employee is null)
            {
                return RedirectToNotFoundError();
            }

            return View(employee);
        }
        catch
        {
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [AdminOnly]
    public async Task<IActionResult> Edit(Employee employee)
    {
        try
        {
            await employeeService.EditAsync(employee);
            return RedirectToAction(nameof(Index));
        }
        catch (LoginAlreadyExistsException)
        {
            TempData[TextFailedEditedEmployeeKey] = "Сотрудник с таким логином уже существует в системе";

            return View(employee);
        }
        catch
        {
            return RedirectToAction(nameof(Index));
        }
    }

    private RedirectToActionResult RedirectToNotFoundError()
    {
        var nameAction = nameof(ErrorsController.EmployeeNotFoundError);

        var fullNameController = nameof(ErrorsController);
        var nameController = NameController.GetControllerName(fullNameController);

        return RedirectToAction(nameAction, nameController);
    }
}