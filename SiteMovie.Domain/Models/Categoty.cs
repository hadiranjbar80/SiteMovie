using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SiteMovie.Domain.Models
{
    public class Category : BaseEntity
    {
        [Display(Name = "عنوان")]
        [Required(ErrorMessage ="لطفا {0} را وارد کنید")]
        public string GroupTitle { get; set; }

        // Navigation properties
        public virtual List<CategoryToMovies> CategoryToMovies { get; set; }
    }
}
