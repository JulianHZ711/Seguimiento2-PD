using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Access.Domain.Entities.Credentials;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Persistence.Configurations
{
    internal class CredentialConfig : IEntityTypeConfiguration<Credential>
    {
        public void Configure(EntityTypeBuilder<Credential> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Code)
                   .HasMaxLength(Credential.CodeMaxLength)
                   .IsRequired();

            builder.HasIndex(c => c.Code)
                   .IsUnique();

            builder.Property(c => c.Status)
                   .IsRequired();

            builder.Property(c => c.CreatedAt)
                   .IsRequired();

            builder.Ignore(c => c.CanBeUsed);

            builder.HasOne(c => c.Employee)
                   .WithMany()
                   .HasForeignKey(c => c.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
