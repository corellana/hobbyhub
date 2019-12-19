using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace Project.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        // First Name ------------------------------------------------------------
        [Required(ErrorMessage = "You must enter your First Name")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
        public string FirstName { get; set; }

        // Last Name ------------------------------------------------------------
        [Required(ErrorMessage = "You must enter your Last Name")]
        [MinLength(2, ErrorMessage = "Your Alias must be at least 2 characters")]
        public string LastName { get; set; }

        // Username ------------------------------------------------------------
        [Required(ErrorMessage = "You must enter your Username")]
        [RegularExpression("^[a-zA-Z]{3,15}$", ErrorMessage = "Your username must be between 3 and 15 characters")]
        public string Username { get; set; }

        // Email ------------------------------------------------------------
        [Required(ErrorMessage = "You must enter an email")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "You need a valid format for your email")]
        public string Email { get; set; }

        // Password ------------------------------------------------------------
        [Required(ErrorMessage = "You must enter an password")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string Password { get; set; }

        // Create and Update  ------------------------------------------------------------
        // The MySQL DATETIME type can be represented by a DateTime
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Will not be mapped to your users table!
        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string Confirm { get; set; }

        // Relationship ------------------------------------------------------------------------------------
        public List<Association> Likes { get; set; }
        
        public List<Hobby> Hobbies { get; set;}
    }
}