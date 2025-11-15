using Microsoft.AspNetCore.Mvc;

namespace TaskManager.View.Controllers;

public class EmployeesController(IEmployeeRepository employeeRepository) : Controller
{
    private const string DefaultPassword = "qwerty123";

    [HttpGet]
    public async Task<IActionResult> Index(Employee? employee)
    {
        var employees = await employeeRepository.GetAllAsync();

        var indexEmployeesViewModel = new IndexEmployeesViewModel
        {
            Employees = employees,
            FailedCreatedEmployee = employee,
        };

        return View(indexEmployeesViewModel);
    }

    [HttpPost]
    [AuthenticationAdmin]
    public async Task<IActionResult> Create(Employee employee)
    {
        var employeeWithSameLogin = await employeeRepository.GetByLoginAsync(employee.Login.ToLower());

        if (employeeWithSameLogin is not null)
        {
            TempData["Error"] = "Пользователь с таким логином уже существует в системе";
            return RedirectToAction(nameof(Index), employee);
        }
        else
        {
            employee.Login = employee.Login.ToLower();
            employee.Password = DefaultPassword;
            employee.Role = RolesDictionary.Employee;

            await employeeRepository.AddAsync(employee);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [AuthenticationAdmin]
    public async Task<IActionResult> Edit(int id)
    {
        var employee = await employeeRepository.GetByIdAsync(id);

        if (employee == null)
        {
            return NotFound();
        }

        return View(employee);
    }

    [HttpPost]
    [AuthenticationAdmin]
    public async Task<IActionResult> Edit(Employee employee)
    {
        await employeeRepository.UpdateAsync(employee);

        return RedirectToAction(nameof(Index));
    }
}