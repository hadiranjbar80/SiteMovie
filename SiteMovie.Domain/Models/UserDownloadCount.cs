using System;
using System.Collections.Generic;
using System.Text;

namespace SiteMovie.Domain.Models
{
    public class UserDownloadCount : BaseEntity
    {
        public string ApplicationUserId { get; set; }
        public int Count { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}
