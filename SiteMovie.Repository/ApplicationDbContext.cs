using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SiteMovie.Domain.EntityMapper;
using SiteMovie.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteMovie.Repository
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Movies> Movies { get; set; }
        public DbSet<MovieLinks> MovieLinks { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryToMovies> CategoryToMovies { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<UserDownloadCount> UserDownloadCounts { get; set; }
        public DbSet<MovieComments> MovieComments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new MovieMap());
            builder.ApplyConfiguration(new CategoryMap());
            builder.ApplyConfiguration(new CategoryToMoviesMap());
            builder.ApplyConfiguration(new MovieLinksMap());
            builder.ApplyConfiguration(new SubscriptionMap());
            builder.ApplyConfiguration(new MovieCommentsMap());
        }
    }
}
