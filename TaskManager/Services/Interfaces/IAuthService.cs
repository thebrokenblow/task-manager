using TaskManager.ViewModel;

namespace TaskManager.Services.Interfaces;

public interface IAuthService
{
    public bool IsAuthenticated { get; }
    public bool IsAdmin { get; }
    public string? FullName { get; }
    public int? CurrentUserId { get; }

    Task<bool> LoginAsync(LoginViewModel loginViewModel);
    Task LogoutAsync();
}