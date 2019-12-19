using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Models
{
    public class Idea
    {
        [Key]
        // Idea ID --------------------------------------------------------------------------------------------
        public int IdeaId { get; set; }

        // Detail Idea -------------------------------------------------------------------------------------------
        [Required(ErrorMessage = "You must enter an Idea")]
        [MinLength(2, ErrorMessage = "Your idea must be at least 5 characters long")]
        public string Detail { get; set; }

        // The MySQL DATETIME type can be represented by a DateTime ----------------------------------------
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Relationship ------------------------------------------------------------------------------------
        public List<Association> Liking { get; set; }

        // Idea Creator -----------------------------------------------------------------------------------------
        public User Creator { get; set; }
        public int UserId {get; set;}





        public bool HasLike(User aUser)
        {
            return Liking.Any(like => like.User.UserId == aUser.UserId);
        }
    }
}


