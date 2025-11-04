using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Services;
using TaskManager.ViewModel;

namespace TaskManager.Controllers;

[AllowAnonymous]
public class AccountsController(AuthService authService) : Controller
{
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (authService.IsAuthenticated())
        {
            return RedirectToAction("Documents", "Index");
        }

        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            return View(loginViewModel);
        }

        var success = await authService.LoginAsync(loginViewModel.Password);

        if (success)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Documents");
        }

        ModelState.AddModelError("", "Неверный пароль");
        return View(loginViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await authService.LogoutAsync();

        return RedirectToAction("Index", "Documents");
    }
}