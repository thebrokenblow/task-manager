using TaskManager.Domain.Entities;

namespace TaskManager.View.ViewModel;

public class IndexEmployeesViewModel
{
    public required List<Employee> Employees { get; init; }
    public Employee? FailedCreatedEmployee { get; init; }
}
