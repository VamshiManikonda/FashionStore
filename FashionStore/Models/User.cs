using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace FashionStore.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        [Display(Name = "State / Province")]
        public string State { get; set; }
        public string Country { get; set; }
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }
        public bool AdminUser { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public int SessionId { get; set; }

        
    }
}