using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Models;

namespace TaskManager.Data.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("employees");

        builder.HasKey(employee => employee.Id);

        builder
            .Property(employee => employee.Id)
            .HasColumnName("id");

        builder
            .Property(employee => employee.FullName)
            .IsRequired()
            .HasColumnName("full_name");

        builder
            .Property(employee => employee.Department)
            .IsRequired()
            .HasColumnName("department");
    }
}