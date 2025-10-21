using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;
using TaskManager.Repositories.Interfaces;

namespace TaskManager.Controllers;

public class EmployeesController(IEmployeeRepository employeeRepository) : Controller
{
    public async Task<IActionResult> Index()
    {
        var employees = await employeeRepository.GetAllAsync();

        return View(employees);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Employee employee)
    {
        await employeeRepository.AddAsync(employee);

        return RedirectToAction(nameof(Index));
    }

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
    public async Task<IActionResult> Edit(Employee employee)
    {
        await employeeRepository.UpdateAsync(employee);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var employee = await employeeRepository.GetByIdAsync(id);

        if (employee == null)
        {
            return NotFound();
        }

        return View(employee);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var employee = await employeeRepository.GetByIdAsync(id);

        if (employee == null)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }
}