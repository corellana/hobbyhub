using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
namespace Project.Models
{
    public class LoginUser
    {
        [Required(ErrorMessage = "You must enter your Email")]
        public string Email {get; set;}
        
        [Required(ErrorMessage = "You must enter your Password")]
        public string Password { get; set; }

        
    }
}