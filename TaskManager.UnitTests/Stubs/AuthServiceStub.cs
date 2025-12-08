using TaskManager.Domain.Model.Employees;
using TaskManager.Domain.Services;

namespace TaskManager.UnitTests.Stubs;

public class AuthServiceStub : IAuthService
{
    public int IdAdmin => default;
    public int? IdCurrentUser => default;

    public bool IsAuthenticated => default;
    public bool IsAdmin => default;

    public string? FullName => default;

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