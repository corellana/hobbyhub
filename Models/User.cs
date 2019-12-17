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
        // MySQL VARCHAR and TEXT types can be represeted by a string

        // Name ------------------------------------------------------------
        [Required(ErrorMessage = "You must enter a Name")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
        // [RegularExpression("^.*(?=.{6,18})(?=.*[A-Za-z]).*$", ErrorMessage = "Your name should be only letters and spaces")]
        public string Name { get; set; }

        // Alias ------------------------------------------------------------
        [Required(ErrorMessage = "You must enter an Alias")]
        [MinLength(2, ErrorMessage = "Your Alias must be at least 2 characters")]
        // [RegularExpression("^.*(?=.{6,18})(?=.*[A-Za-z]).*$", ErrorMessage = "Your alias should be letters and numbers only")]
        public string Alias { get; set; }

        // Email ------------------------------------------------------------
        [Required(ErrorMessage = "You must enter an email")]
        // [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
        public string Email { get; set; }

        // Password ------------------------------------------------------------
        [Required(ErrorMessage = "You must enter an password")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        // [RegularExpression("^.*(?=.{6,18})(?=.*[0-9])(?=.*[A-Za-z])(?=.*[@%&#]{1,}).*$", ErrorMessage = "Your password need at least a letter, a number and a special character")]
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
        // public List<Association> Weddings { get; set; }
    }
}