using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Domain.Exceptions;
using TaskManager.Domain.Model.Employees;
using TaskManager.Domain.Queries;
using TaskManager.Domain.Repositories;
using TaskManager.Domain.Services;
using TaskManager.View.Filters;
using TaskManager.View.Utilities;
using TaskManager.View.ViewModel.Employees;

namespace TaskManager.View.Controllers;

[AllowAnonymous]
public sealed class AccountsController(
    IEmployeeQuery employeeQuery,
    IEmployeeRepository employeeRepository, 
    IAuthService authService) : Controller
{
    private readonly IEmployeeQuery _employeeQuery =
            employeeQuery ?? throw new ArgumentNullException(nameof(employeeQuery));

    private readonly IEmployeeRepository _employeeRepository =
            employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));

    private readonly IAuthService _authService =
            authService ?? throw new ArgumentNullException(nameof(authService));

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (_authService.IsAuthenticated)
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

        var success = await _authService.LoginAsync(employeeLoginModel);

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
        await _authService.LogoutAsync();
        return RedirectToDocuments();
    }

    [HttpGet]
    [CheckUserAccess]
    public async Task<IActionResult> ChangePassword(int id)
    {
        if (id != authService.IdCurrentUser)
        {
            var nameAction = nameof(ErrorsController.AccessDeniedError);
            var fullNameController = nameof(ErrorsController);

            var nameController = NameController.GetControllerName(fullNameController);

            return RedirectToAction(nameAction, nameController);
        }

        var isExist = await _employeeRepository.IsExistAsync(id);
        if (!isExist)
        {
            return RedirectToNotFoundError();
        }

        var password = await _employeeQuery.GetPasswordAsync(id);
        if (string.IsNullOrEmpty(password))
        {
            return RedirectToNotFoundError();
        }

        var passwordViewModel = new EmployeePasswordViewModel
        {
            Id = id,
            Password = password
        };

        return View(passwordViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> ChangePasswordConfirmed(EmployeePasswordViewModel passwordViewModel)
    {
        try
        {
            await _employeeRepository.UpdatePasswordAsync(passwordViewModel.Id, passwordViewModel.Password);
        }
        catch (NotFoundException)
        {
            return RedirectToNotFoundError();
        }

        return RedirectToDocuments();
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