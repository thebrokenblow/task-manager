using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TaskManager.Application.Dtos.Employees;
using TaskManager.Application.Exceptions;
using TaskManager.Application.Services.Interfaces;
using TaskManager.View.Filters;
using TaskManager.View.Utilities;
using TaskManager.View.ViewModel.Employees;

namespace TaskManager.View.Controllers;

public sealed class EmployeesController(IEmployeeService employeeService) : Controller
{
    private readonly IEmployeeService _employeeService =
        employeeService ?? throw new ArgumentNullException(nameof(employeeService));

    private const string FailedCreatedEmployeeKey = "FailedCreatedEmployee";
    private const string TextFailedCreatedEmployeeKey = "TextFailedCreatedEmployee";
    
    public const string TextFailedEditedEmployeeKey = "TextFailedEditedEmployeeKey";

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var employees = await _employeeService.GetRegularEmployeesAsync();

        var indexEmployeesViewModel = new IndexEmployeesViewModel
        {
            Employees = employees
        };

        if (TempData.TryGetValue(FailedCreatedEmployeeKey, out var failedEmployeeJson) &&
            TempData.TryGetValue(TextFailedCreatedEmployeeKey, out var textFailed))
        {
            if (failedEmployeeJson is not null)
            {
                var failedEmployee = JsonSerializer.Deserialize<CreatedEmployeeDto>((string)failedEmployeeJson);
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

    [HttpPost]
    public async Task<IActionResult> Create(CreatedEmployeeDto createdEmployeeDto)
    {
        try
        {
            await _employeeService.CreateAsync(createdEmployeeDto);
            return RedirectToAction(nameof(Index));
        }
        catch (LoginAlreadyExistsException)
        {
            TempData[FailedCreatedEmployeeKey] = JsonSerializer.Serialize(createdEmployeeDto);
            TempData[TextFailedCreatedEmployeeKey] = "Сотрудник с таким логином уже существует в системе";
        }
        catch
        {
            TempData[TextFailedCreatedEmployeeKey] = "Произошла ошибка при создании сотрудника, обратитесь к администратору";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [AdminOnly]
    public async Task<IActionResult> Edit(int id)
    {
        var employee = await _employeeService.GetEmployeeForEditAsync(id);

        if (employee is null)
        {
            return RedirectToNotFoundError();
        }

        return View(employee);
    }

    [HttpPost]
    [AdminOnly]
    public async Task<IActionResult> Edit(EditedEmployeeDto editedEmployeeDto)
    {
        try
        {
            await _employeeService.EditAsync(editedEmployeeDto);
            return RedirectToAction(nameof(Index));
        }
        catch (LoginAlreadyExistsException)
        {
            TempData[TextFailedEditedEmployeeKey] = "Сотрудник с таким логином уже существует в системе";

            return View(editedEmployeeDto);
        }
        catch
        {
            TempData[TextFailedEditedEmployeeKey] = "Проблема с сохранением данных сотрудника, обратитесь в администратору";

            return View(editedEmployeeDto);
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