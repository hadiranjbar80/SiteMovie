using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SiteMovie.Domain.ViewModels
{
    public class CreateCommentViewModel
    {
        public string UserId { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Comment { get; set; }
        public int MovieId { get; set; }
        public int Rating { get; set; }
        public bool IsActive { get; set; }
    }

    public class ShowCommentsViewModel
    {
        public string UserName { get; set; }
        public DateTime CreateDate { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
    }
}
