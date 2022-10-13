using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace WallStreetBets.Models
{
    public class User
    {
        [Key]
        public int UserId {get; set;}

        [Required]
        [MinLength(2, ErrorMessage = "must be at least 2 Characters")]
        [Display(Name = "First Name")]
        public string FirstName {get; set;}

        [Required]
        [MinLength(2, ErrorMessage = "must be at least 2 Characters")]
        [Display(Name = "Last Name")]
        public string LastName {get; set;}

        [Required(ErrorMessage = "is required")]
        [EmailAddress]
        [CustomValidations.UniqueEmail]
        public string Email {get; set;}

        [Required]
        [MinLength(8, ErrorMessage = "must be at least 8 characters")]
        [DataType(DataType.Password)]
        public string Password {get; set;}

        [NotMapped]
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "doesnt match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword {get; set;}

        public DateTime CreatedAt {get; set;} = DateTime.Now;
        public DateTime UpdatedAt {get; set;} = DateTime.Now;


        public List<ListStock> CreatedListStock { get; set; }
    }
    
}