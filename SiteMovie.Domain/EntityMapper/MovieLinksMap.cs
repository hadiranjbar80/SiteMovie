using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SiteMovie.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiteMovie.Domain.EntityMapper
{
    public class MovieLinksMap : IEntityTypeConfiguration<MovieLinks>
    {
        public void Configure(EntityTypeBuilder<MovieLinks> builder)
        {
            builder.HasKey(ml => ml.Id);
            builder.Property(m => m.Id).ValueGeneratedOnAdd();
            builder.Property(ml => ml.Attachment)
                .HasColumnType("varchar(50)")
                .HasColumnName("Attachment")
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(ml => ml.LinkTitle)
                .HasColumnName("LinkTitle")
                .HasColumnType("nvarchar(150)")
                .HasMaxLength(150)
                .IsRequired();
        }
    }
}
