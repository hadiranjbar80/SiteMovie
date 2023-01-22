using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SiteMovie.Domain.ViewModels
{
    public class AddLinkViewModel
    {
        public int MovieId { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "فایل")]
        public IFormFile Attachment { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "عنون لینک")]
        public string LinkTitle { get; set; }

    }

    public class EditLinkViewModel
    {
        public int MovieId { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "فایل")]
        public IFormFile Attachment { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "عنون لینک")]
        public string LinkTitle { get; set; }

    }

    public class ListMovieLinksViewModel
    {
        public int Id { get; set; }
        [Display(Name = "فایل")]
        public string Attachment { get; set; }
        [Display(Name = "عنوان لینک")]
        public string LinkTitle { get; set; }
    }
}
