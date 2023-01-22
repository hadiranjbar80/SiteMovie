using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SiteMovie.Domain.ViewModels
{
    public class ShowUserProfileViewModel
    {
        public string Email { get; set; }
        public string ImageName { get; set; }
        public string ExpireDateSubscription { get; set; }
        public string UserName { get; set; }
        public string SubscriptionTitle { get; set; }
        public bool IsFinally { get; set; }
    }

    public class UsersIndexViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }

    public class UserRolesViewModel
    {
        #region Constructor
        public UserRolesViewModel()
        {

        }

        public UserRolesViewModel(string roleName)
        {
            RoleName = roleName;
        }
        #endregion
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }

    public class AddUserToRoleViewModel
    {
        #region Constructor

        public AddUserToRoleViewModel()
        {
            UserRoles = new List<UserRolesViewModel>();
        }

        public AddUserToRoleViewModel(string userId, IList<UserRolesViewModel> userRoles)
        {
            UserId = userId;
            UserRoles = userRoles;
        }

        #endregion

        public string UserId { get; set; }
        public IList<UserRolesViewModel> UserRoles { get; set; }
    }

    public class EditProfileViewModel
    {
        public string UserId { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public IFormFile ImageName { get; set; }

        [Display(Name ="ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [DataType(DataType.EmailAddress, ErrorMessage = "ایمیل وارد شده معتبر نمی‌باشد")]
        public string Email { get; set; }

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string UserName { get; set; }

    }
}
