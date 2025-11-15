namespace TaskManager.Domain.Models;

public class EmployeeForSelect
{
    public required int Id { get; init; }
    public required string FullNameAndDepartment { get; init; }
}