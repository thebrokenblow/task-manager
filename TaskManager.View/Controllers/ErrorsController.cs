using Microsoft.AspNetCore.Mvc;

namespace TaskManager.View.Controllers;

public class ErrorsController : Controller
{
    [HttpGet]
    public IActionResult UnhandledError()
    {
        return View();
    }

    [HttpGet]
    public IActionResult DocumentNotFoundError()
    {
        return View();
    }

    [HttpGet]
    public IActionResult EmployeeNotFoundError()
    {
        return View();
    }

    [HttpGet]
    public IActionResult AccessDeniedError()
    {
        return View();
    }
}