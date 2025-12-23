namespace TaskManager.Application.Dtos.Employees;

public sealed class EditedEmployeeDto
{
    public required int Id { get; init; }
    public required string FullName { get; set; }
    public required string Department { get; set; }
    public required string Login { get; set; }
}