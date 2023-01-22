using System;
using System.Collections.Generic;
using System.Text;

namespace SiteMovie.Domain.Models
{
    public class CategoryToMovies
    {
        public int MovieId { get; set; }
        public int CategoryId { get; set; }

        // Navigation properties
        public virtual Category Category { get; set; }
        public virtual Movies Movies { get; set; }
    }
}
