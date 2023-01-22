using System;
using System.Collections.Generic;
using System.Text;

namespace SiteMovie.Domain.Models
{
    public class Subscription : BaseEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public bool IsFinally { get; set; }
        public string ApplicationUserId { get; set; }

        // Navigation properties
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
