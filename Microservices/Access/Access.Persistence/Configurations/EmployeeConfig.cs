using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Access.Domain.Entities.Employees;
using Access.Domain.Entities.Employees.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Persistence.Configurations
{
    internal class EmployeeConfig : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.DocumentNumber)
                   .HasConversion(document => document.Value, value => new DocumentNumber(value))
                   .HasMaxLength(DocumentNumber.MaxLength)
                   .IsRequired();

            builder.HasIndex(e => e.DocumentNumber)
                   .IsUnique();

            builder.Property(e => e.Email)
                   .HasConversion(email => email.Value, value => new Email(value))
                   .HasMaxLength(Email.MaxLength)
                   .IsRequired();

            builder.HasIndex(e => e.Email)
                   .IsUnique();

            builder.Property(e => e.FirstName)
                   .HasMaxLength(Employee.NameMaxLength)
                   .IsRequired();

            builder.Property(e => e.LastName)
                   .HasMaxLength(Employee.NameMaxLength)
                   .IsRequired();

            builder.Property(e => e.Position)
                   .HasMaxLength(Employee.PositionMaxLength)
                   .IsRequired();

            builder.Property(e => e.Status)
                   .IsRequired();

            builder.Property(e => e.CreatedAt)
                   .IsRequired();

            builder.Ignore(e => e.IsActive);

            builder.HasOne(e => e.Area)
                   .WithMany(a => a.Employees)
                   .HasForeignKey(e => e.AreaId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
