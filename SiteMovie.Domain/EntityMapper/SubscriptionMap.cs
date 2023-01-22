using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SiteMovie.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiteMovie.Domain.EntityMapper
{
    public class SubscriptionMap : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.ApplicationUserId)
                .HasColumnType("nvarchar(450)")
                .IsRequired()
                .HasMaxLength(450);
            builder.Property(s => s.Title)
                .HasColumnType("nvarchar(50)")
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("Title");
            builder.Property(s => s.Price)
                .IsRequired();
        }
    }
}
