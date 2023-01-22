using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SiteMovie.Domain.Models
{
    public class MovieComments
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Comment { get; set; }
        public DateTime CreateDate { get; set; }
        [ForeignKey("MovieId")]
        public int MovieId { get; set; }
        public int Rating { get; set; }
        public string ApplicationUserName { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public virtual Movies Movies { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
