using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SiteMovie.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiteMovie.Domain.EntityMapper
{
    public class MovieMap : IEntityTypeConfiguration<Movies>
    {
        public void Configure(EntityTypeBuilder<Movies> builder)
        {
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Id).ValueGeneratedOnAdd();
            builder.Property(m => m.Actors)
                .HasColumnType("nvarchar(500)")
                .HasColumnName("Actors")
                .HasMaxLength(500)
                .IsRequired();
            builder.Property(m => m.Country)
                .HasColumnType("nvarchar(50)")
                .HasColumnName("Country")
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(m => m.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("CreateDate")
                .IsRequired();
            builder.Property(m => m.Description)
                .HasColumnType("nvarchar(500)")
                .HasColumnName("Description")
                .HasMaxLength(500)
                .IsRequired();
            builder.Property(m => m.Director)
                .HasColumnType("nvarchar(50)")
                .HasColumnName("Director")
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(m => m.MovieTitle)
                .HasColumnType("nvarchar(100)")
                .HasColumnName("MovieTitle")
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(m => m.MovieImage)
                .HasColumnType("varchar(50)")
                .HasColumnName("MovieImage")
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(m => m.PublishDate)
                .HasColumnType("nvarchar(200)")
                .HasColumnName("PublishDate")
                .IsRequired();
            builder.Property(m => m.imdbRating)
               .HasColumnName("imdbRating");
        }
    }
}
