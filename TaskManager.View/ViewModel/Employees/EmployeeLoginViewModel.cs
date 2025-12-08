namespace TaskManager.View.ViewModel.Employees;

public class EmployeeLoginViewModel
{
    public required string Login { get; set; }
    public required string Password { get; set; }

    public string? ReturnUrl { get; set; }
}