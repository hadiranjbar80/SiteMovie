using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SiteMovie.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Subscription Subscription { get; set; }
        public UserDownloadCount UserDownloadCount { get; set; }
        public List<MovieComments> MovieComments { get; set; }
        public string ImageName { get; set; }
    }
}
