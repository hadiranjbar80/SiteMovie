using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteMovie.Presentation
{
    public class PersianIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateEmail(string email)
            => new IdentityError()
            {
                Code = nameof(DuplicateEmail)
                ,
                Description = $"ایمیل '{email}' قبلا توسط شخص دیگری انتخاب شده است."
            };

        public override IdentityError DuplicateUserName(string userName)
            => new IdentityError()
            {
                Code = nameof(DuplicateUserName),
                Description = $"نام کاربری '{userName}' قبلا توسط شخص دیگری انتخاب شده است."
            };

        public override IdentityError PasswordTooShort(int length)
          => new IdentityError()
          {
              Code = nameof(PasswordTooShort),
              Description = $"رمز عبور نمی‌تواند کمتر از '{length}' رقم باشد."
          };

        public override IdentityError PasswordRequiresUpper()
            => new IdentityError()
            {
                Code = nameof(PasswordRequiresUpper),
                Description = $"حداقل یک حرف بزرگ انگلیسی باید در رمز عبور وجود داشته باشد."
            };

        public override IdentityError PasswordRequiresLower()
        => new IdentityError()
        {
            Code = nameof(PasswordRequiresLower),
            Description = $"حداقل یک حرف کوچک انگلیسی باید در رمز عبور وجود داشته باشد."
        };

        public override IdentityError PasswordRequiresDigit()
       => new IdentityError()
       {
           Code = (nameof(PasswordRequiresDigit)),
           Description = $"رمز عبور حداقل باید شامل یک عدد باشد."
       };
    }
}
