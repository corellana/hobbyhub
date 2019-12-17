using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Models
{
    public class CdActivity
    {
        // auto-implemented properties need to match the columns in your table
        // the [Key] attribute is used to mark the Model property being used for your table's Primary Key
        [Key]
        // ID --------------------------------------------------------------------------------------------
        public int CdActivityId { get; set; }

        // Title -------------------------------------------------------------------------------------------
        [Required(ErrorMessage = "You must enter a Title")]
        [MinLength(2, ErrorMessage = "Title must be at least 2 characters")]
        public string Title { get; set; }

        // Date ------------------------------------------------------------
        [Required(ErrorMessage = "You must enter a Date")]
        // [Display(Name = "Date of Birth:")]
        [DataType(DataType.Date)]
        [DateInTheFuture]
        // [MinimumAge(18)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        // Date ------------------------------------------------------------
        [Required(ErrorMessage = "You must enter a Time")]
        // [Display(Name = "Date of Birth:")]
        [DataType(DataType.Time)]
        // [MinimumAge(18)]
        [DisplayFormat(DataFormatString = "{0:H:mm}", ApplyFormatInEditMode = true)]
        public DateTime Time { get; set; }

        // Duration -------------------------------------------------------------------------------------------
        [Required(ErrorMessage = "You must enter a duration")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than 0")]
        // [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
        public int Duration { get; set; }

        // Description -------------------------------------------------------------------------------------------
        [Required(ErrorMessage = "You must enter a Description")]
        [MinLength(2, ErrorMessage = "Descriptiom must be at least 2 characters")]
        public string Description { get; set; }

        // The MySQL DATETIME type can be represented by a DateTime ----------------------------------------
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Relationship ------------------------------------------------------------------------------------
        public List<Association> Guests { get; set; }

        // Creator -----------------------------------------------------------------------------------------
        public User Creator { get; set; }
        public int UserId {get; set;}

        public bool HasGuest(User guest)
        {
            return Guests.Any(g => g.User.UserId == guest.UserId);
        }

        internal bool Overlaps(CdActivity theCdActivity)
        {
            return false;
        }
    }
}


