using TaskManager.Application.Services.Interfaces;
using TaskManager.Domain.Interfaces.Repositories;
using TaskManager.Domain.Models;

namespace TaskManager.Application.Services;

public class AccountService(IEmployeeRepository employeeRepository) : IAccountService
{
    public async Task ChangePasswordAsync(int id, PasswordModel passwordViewModel)
    {
        var employee = await employeeRepository.GetByIdAsync(id);

        employee.Password = passwordViewModel.Password;
        await employeeRepository.UpdateAsync(employee);
    }
}