using TaskManager.Domain.Entities;

namespace TaskManager.View.ViewModel.Employees;

public class IndexEmployeesViewModel
{
    public required List<Employee> Employees { get; init; }
    public Employee? FailedCreatedEmployee { get; set; }
    public string? TextFailedCreatedEmployee { get; set; }
}