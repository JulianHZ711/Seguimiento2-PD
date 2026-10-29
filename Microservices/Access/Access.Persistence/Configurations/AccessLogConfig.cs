using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Access.Domain.Entities.AccessLogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Persistence.Configurations
{
    internal class AccessLogConfig : IEntityTypeConfiguration<AccessLog>
    {
        public void Configure(EntityTypeBuilder<AccessLog> builder)
        {
            builder.HasKey(l => l.Id);

            builder.Property(l => l.CredentialCode)
                   .HasMaxLength(AccessLog.CredentialCodeMaxLength)
                   .IsRequired();

            builder.Property(l => l.MovementType)
                   .IsRequired();

            builder.Property(l => l.Result)
                   .IsRequired();

            builder.Property(l => l.RejectionReason)
                   .HasMaxLength(AccessLog.RejectionReasonMaxLength);

            builder.Property(l => l.OccurredAt)
                   .IsRequired();

            // El historial no se borra: ninguna relación elimina registros en cascada.
            builder.HasOne(l => l.Credential)
                   .WithMany()
                   .HasForeignKey(l => l.CredentialId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(l => l.Employee)
                   .WithMany()
                   .HasForeignKey(l => l.EmployeeId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(l => l.AccessPoint)
                   .WithMany()
                   .HasForeignKey(l => l.AccessPointId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Índices para las consultas del historial (RF-04).
            builder.HasIndex(l => new { l.EmployeeId, l.OccurredAt });
            builder.HasIndex(l => new { l.AccessPointId, l.OccurredAt });
            builder.HasIndex(l => l.OccurredAt);
        }
    }
}
