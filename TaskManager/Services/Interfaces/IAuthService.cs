using TaskManager.ViewModel;

namespace TaskManager.Services.Interfaces;

public interface IAuthService
{
    bool IsAuthenticated { get; }
    bool IsAdmin { get; }
    int? CurrentUserId { get; }

    Task LogoutAsync();
    Task<bool> LoginAsync(LoginViewModel loginViewModel);
}