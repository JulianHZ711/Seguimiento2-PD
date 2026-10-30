using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Access.Domain.Entities.AccessPoints;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Persistence.Configurations
{
    internal class AccessPointConfig : IEntityTypeConfiguration<AccessPoint>
    {
        public void Configure(EntityTypeBuilder<AccessPoint> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                   .HasMaxLength(AccessPoint.NameMaxLength)
                   .IsRequired();

            builder.HasIndex(p => p.Name)
                   .IsUnique();

            builder.Property(p => p.Type)
                   .IsRequired();

            builder.Property(p => p.IsEnabled)
                   .IsRequired();

            builder.Property(p => p.CreatedAt)
                   .IsRequired();
        }
    }
}
