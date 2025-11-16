using TaskManager.Domain.Models;

namespace TaskManager.Domain.Interfaces.Services;

public interface IAuthService
{
    public bool IsAuthenticated { get; }
    public bool IsAdmin { get; }
    public string? FullName { get; }
    public int AdminId { get; } 
    public int? CurrentUserId { get; }

    Task LogoutAsync();
    Task<bool> LoginAsync(LoginModel loginViewModel);
}