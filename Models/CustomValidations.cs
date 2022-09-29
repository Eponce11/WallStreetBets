using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WallStreetBets.Models
{
    public class CustomValidations
    {

        public class UniqueEmail : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                MyContext db = (MyContext)validationContext.GetService(typeof(MyContext)); // gets access to the db through mycontext

                bool isEmailTaken = db.Users.Any(u => u.Email == (string)value);

                if (isEmailTaken)
                {
                    return new ValidationResult("Email already taken");
                }

                return ValidationResult.Success;
            }
        }
        
    }
}