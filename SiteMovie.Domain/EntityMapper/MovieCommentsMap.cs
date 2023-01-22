using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SiteMovie.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiteMovie.Domain.EntityMapper
{
    public class MovieCommentsMap : IEntityTypeConfiguration<MovieComments>
    {
        public void Configure(EntityTypeBuilder<MovieComments> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.Property(c => c.ApplicationUserName)
                .HasMaxLength(70)
                .HasColumnType("nvarchar(70)")
                .IsRequired();

            builder.Property(c => c.Comment)
                .HasColumnName("Commnet")
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)")
                .IsRequired();
        }
    }
}
