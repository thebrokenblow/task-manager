using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Domain.Interfaces.Repositories;
using TaskManager.Domain.Interfaces.Services;
using TaskManager.Domain.Models;
using TaskManager.View.Utils;
using TaskManager.View.ViewModel;

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

        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel loginModel, string? returnUrl = null)
    {
        var success = await authService.LoginAsync(loginModel);

        if (success)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToDocuments();
        }

        ModelState.AddModelError("", "Неверный логин или пароль");
        return View(loginModel);
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
            return NotFound();
        }

        var passwordViewModel = new PasswordViewModel
        {
            Id = id,
            Password = employee.Password
        };

        return View(passwordViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> ChangePasswordConfirmed(int id, PasswordViewModel passwordViewModel)
    {
        var employee = await employeeRepository.GetByIdAsync(id);

        if (employee is null)
        {
            return NotFound();
        }

        employee.Password = passwordViewModel.Password;
        await employeeRepository.UpdateAsync(employee);

        return RedirectToDocuments();
    }

    private RedirectToActionResult RedirectToDocuments()
    {
        var nameAction = nameof(DocumentsController.Index);
        var fullNameController = nameof(DocumentsController);
        var nameController = NameController.GetControllerName(fullNameController);

        return RedirectToAction(nameAction, nameController);
    }
}