using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace Project.Models
{
    public class User
    {
        // auto-implemented properties need to match the columns in your table
        // the [Key] attribute is used to mark the Model property being used for your table's Primary Key
        [Key]
        public int UserId { get; set; }
        // MySQL VARCHAR and TEXT types can be represeted by a string

        // Name ------------------------------------------------------------
        [Required(ErrorMessage = "You must enter a Name")]
        [MinLength(2, ErrorMessage = "First Name must be at least 2 characters")]
        public string Name { get; set; }

        // Email ------------------------------------------------------------
        [Required(ErrorMessage = "You must enter an email")]
        // [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
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
        // public List<Association> Weddings { get; set; }
    }
}