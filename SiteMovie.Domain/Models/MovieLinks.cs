using System;
using System.Collections.Generic;
using System.Text;

namespace SiteMovie.Domain.Models
{
    public class MovieLinks : BaseEntity
    {
        public int MovieId { get; set; }
        public string Attachment { get; set; }
        public string LinkTitle { get; set; }

        // Navigation properties
        public virtual Movies Movies { get; set; }
    }
}
