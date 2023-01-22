using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SiteMovie.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiteMovie.Domain.EntityMapper
{
    public class CategoryToMoviesMap : IEntityTypeConfiguration<CategoryToMovies>
    {
        public void Configure(EntityTypeBuilder<CategoryToMovies> builder)
        {
            builder.HasKey(cm => new { cm.CategoryId, cm.MovieId });

        }
    }
}
