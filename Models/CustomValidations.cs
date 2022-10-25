using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WallStreetBets.Models
{
    public class CustomValidations
    {
        
        // validation that checks if the email is already in use
        public class UniqueEmail : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                // access to the database
                MyContext db = (MyContext)validationContext.GetService(typeof(MyContext)); // gets access to the db through mycontext

                // checks if the emaiL is in the db
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