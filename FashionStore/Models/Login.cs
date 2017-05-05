using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FashionStore.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Required")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        [Display(Name = "State / Province")]
        public string State { get; set; }
        public string Country { get; set; }
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Email { get; set; }
        [Display(Name = "Admin User")]
        public bool AdminUser { get; set; }
        public User User { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
    }
}