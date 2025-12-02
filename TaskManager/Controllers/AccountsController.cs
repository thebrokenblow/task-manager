using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Domain.Model.Employees;
using TaskManager.Domain.Repositories;
using TaskManager.Domain.Services;
using TaskManager.View.Utils;
using TaskManager.View.ViewModel.Employees;

namespace TaskManager.View.Controllers;

[AllowAnonymous]
public class AccountsController(
    IEmployeeRepository employeeRepository, 
    IAuthService authService) : Controller
{
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (authService.IsAuthenticated)
        {
            return RedirectToDocuments();
        }

        var employeeLoginViewModel = new EmployeeLoginViewModel
        {
            Login = string.Empty,
            Password = string.Empty,
            ReturnUrl = returnUrl
        };

        return View(employeeLoginViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Login(EmployeeLoginViewModel employeeLoginViewModel, string? returnUrl = null)
    {
        employeeLoginViewModel.ReturnUrl = returnUrl;

        var employeeLoginModel = new EmployeeLoginModel
        {
            Login = employeeLoginViewModel.Login,
            Password = employeeLoginViewModel.Password,
        };

        var success = await authService.LoginAsync(employeeLoginModel);

        if (success)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToDocuments();
        }

        ModelState.AddModelError("", "Неверный логин или пароль");
        return View(employeeLoginViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await authService.LogoutAsync();
        return RedirectToDocuments();
    }

    [HttpGet]
    public async Task<IActionResult> ChangePassword(int id)
    {
        var employee = await employeeRepository.GetByIdAsync(id);

        if (employee == null)
        {
            return RedirectToNotFoundError();
        }

        var passwordViewModel = new EmployeePasswordViewModel
        {
            Id = id,
            Password = employee.Password
        };

        return View(passwordViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> ChangePasswordConfirmed(int id, EmployeePasswordViewModel passwordViewModel)
    {
        var employee = await employeeRepository.GetByIdAsync(id);

        if (employee is null)
        {
            return RedirectToNotFoundError();
        }

        employee.Password = passwordViewModel.Password;
        await employeeRepository.UpdateAsync(employee);

        return RedirectToDocuments();
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }

    private RedirectToActionResult RedirectToNotFoundError()
    {
        var nameAction = nameof(ErrorsController.EmployeeNotFoundError);

        var fullNameController = nameof(ErrorsController);
        var nameController = NameController.GetControllerName(fullNameController);

        return RedirectToAction(nameAction, nameController);
    }

    private RedirectToActionResult RedirectToDocuments()
    {
        var nameAction = nameof(DocumentsController.Index);
        var fullNameController = nameof(DocumentsController);

        var nameController = NameController.GetControllerName(fullNameController);

        return RedirectToAction(nameAction, nameController);
    }
}