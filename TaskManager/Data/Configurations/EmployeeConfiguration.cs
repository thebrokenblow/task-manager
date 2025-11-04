using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Models;

namespace TaskManager.Data.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder
            .ToTable("employees");

        builder
            .HasKey(employee => employee.Id);

        builder
            .Property(employee => employee.Id)
            .HasColumnName("id")
            .UseIdentityByDefaultColumn();

        builder
            .Property(employee => employee.FullName)
            .HasColumnName("full_name")
            .IsRequired();

        builder
            .Property(employee => employee.Department)
            .HasColumnName("department")
            .IsRequired();
    }
}