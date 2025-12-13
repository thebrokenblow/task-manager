using TaskManager.Domain.Enums;
using TaskManager.Domain.Model.Employees;
using TaskManager.Domain.Services;

namespace TaskManager.UnitTests.Stubs;

public class AuthServiceStub : IAuthService
{
    public int IdAdmin => 1;
    public int? IdCurrentUser => null;

    public bool IsAuthenticated => default;
    public bool IsAdmin => default;

    public string? FullName => null;

    public UserRole? Role => null;

    public async Task<bool> LoginAsync(EmployeeLoginModel employeeLoginModel)
    {
        await Task.CompletedTask; 
        throw new NotImplementedException("Метод LoginAsync не реализован");
    }

    public Task LogoutAsync()
    {
        throw new NotImplementedException("Метод LogoutAsync не реализован");
    }
}