using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Access.Domain.Entities.Areas;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access.Persistence.Configurations
{
    internal class AreaConfig : IEntityTypeConfiguration<Area>
    {
        public void Configure(EntityTypeBuilder<Area> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Name)
                   .HasMaxLength(Area.NameMaxLength)
                   .IsRequired();

            builder.Property(a => a.Description)
                   .HasMaxLength(Area.DescriptionMaxLength);
        }
    }
}
