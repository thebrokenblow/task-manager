namespace TaskManager.ViewModel;

public class LoginViewModel
{
    public required string Login { get; set; }
    public required string Password { get; set; }

    public string? ReturnUrl { get; set; }
}