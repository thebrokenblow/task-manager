using TaskManager.Application.Services.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Entities.Dictionaries;
using TaskManager.Domain.Interfaces.Repositories;

namespace TaskManager.Application.Services;

public class EmployeeService(IEmployeeRepository employeeRepository) : IEmployeeService
{
    private const string DefaultPassword = "qwerty123";

    public async Task<List<Employee>> GetAllAsync()
    {
        var employees = await employeeRepository.GetAllAsync();

        return employees;
    }

    public async Task CreateAsync(Employee employee)
    {
        var login = employee.Login.ToLower().Trim();
        var employeeWithSameLogin = await employeeRepository.GetByLoginAsync(login);

        if (employeeWithSameLogin is not null)
        {
            //"Пользователь с таким логином уже существует в системе"
            throw new Exception();
        }

        employee.Login = login;
        employee.Password = DefaultPassword;
        employee.Role = RolesDictionary.Employee;

        employee = TrimEmployee(employee);

        await employeeRepository.AddAsync(employee);
    }

    public async Task EditAsync(Employee employee)
    {
        var login = employee.Login.ToLower().Trim();
        var employeeWithSameLogin = await employeeRepository.GetByLoginAsync(login);

        if (employeeWithSameLogin is not null)
        {
            //"Пользователь с таким логином уже существует в системе"
            throw new Exception();
        }

        employee.Login = login;
        employee = TrimEmployee(employee);

        await employeeRepository.UpdateAsync(employee);
    }

    private static Employee TrimEmployee(Employee employee)
    {
        employee.FullName = employee.FullName.Trim();
        employee.Department = employee.Department.Trim();

        return employee;
    }
}