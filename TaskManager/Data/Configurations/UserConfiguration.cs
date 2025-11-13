using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Models;

namespace TaskManager.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .ToTable("users");

        builder
            .HasKey(user => user.Id);

        builder
            .Property(user => user.Id)
            .HasColumnName("id");

        builder
           .Property(user => user.Login)
           .IsRequired()
           .HasColumnName("login");

        builder
            .Property(user => user.Password)
            .IsRequired()
            .HasColumnName("password");
    }
}