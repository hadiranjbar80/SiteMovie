using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiteMovie.Domain.Models
{
    public class Movies : BaseEntity
    {
        public string Actors { get; set; }
        public string Country { get; set; }
        // It's the time whene the post is created
        public DateTime CreateDate { get; set; }
        public string Description { get; set; }
        public string Director { get; set; }
        public string MovieTitle { get; set; }
        public string MovieImage { get; set; }
        // It's the the time whene the movie is published  
        public string PublishDate { get; set; }
        public string Demo { get; set; }
        public decimal imdbRating { get; set; }
        public int Visit { get; set; }

        // Navigation properties
        public virtual List<MovieLinks> MovieLinks { get; set; }
        public virtual List<CategoryToMovies> CategoryToMovies { get; set; }
        public virtual List<MovieComments> MovieComments { get; set; }
    }
}
