using TaskManager.Models;

namespace TaskManager.ViewModel;

public class IndexEmployeesViewModel
{
    public required List<Employee> Employees { get; init; }
    public Employee? FailedCreatedEmployee { get; init; }
}
