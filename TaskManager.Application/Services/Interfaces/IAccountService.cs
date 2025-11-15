using TaskManager.Domain.Models;

namespace TaskManager.Application.Services.Interfaces;

public interface IAccountService
{
    Task ChangePasswordAsync(int id, PasswordModel passwordViewModel);
}