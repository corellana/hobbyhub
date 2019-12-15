using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Models
{
}



// Tu Plantilla para la ACTIVIDAD
//     public class Wedding
//     {
//         // auto-implemented properties need to match the columns in your table
//         // the [Key] attribute is used to mark the Model property being used for your table's Primary Key
//         [Key]
//         // ID --------------------------------------------------------------------------------------------
//         public int WeddingId { get; set; }

//         // Wedder One -------------------------------------------------------------------------------------------
//         [Required(ErrorMessage = "You must enter a name")]
//         // [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
//         public string NameWedderOne { get; set; }

//         // Wedder Two -------------------------------------------------------------------------------------------
//         [Required(ErrorMessage = "You must enter a name")]
//         // [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
//         public string NameWedderTwo { get; set; }

//         // Date ------------------------------------------------------------
//         [Required(ErrorMessage = "You must enter a Date")]
//         // [MinLength(2, ErrorMessage = "Date of Birth must be a past date")]
//         // [Display(Name = "Date of Birth:")]
//         [DataType(DataType.Date)]
//         [DateInTheFuture]
//         // [MinimumAge(18)]
//         [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
//         public DateTime Day { get; set; }

//         // Address -------------------------------------------------------------------------------------------
//         [Required(ErrorMessage = "You must enter a Address")]
//         // [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
//         public string Address { get; set; }

        

//         // The MySQL DATETIME type can be represented by a DateTime ----------------------------------------
//         public DateTime CreatedAt { get; set; }
//         public DateTime UpdatedAt { get; set; }

//         // Relationship ------------------------------------------------------------------------------------
//         public List<Association> Guests { get; set; }

//         // Creator -----------------------------------------------------------------------------------------
//         public User Creator { get; set; }
//         public int UserId {get; set;}

//         public bool HasGuest(User guest)
//         {
//             return Guests.Any(g => g.User.UserId == guest.UserId);
//         }
//     }

