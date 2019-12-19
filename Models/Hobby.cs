using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Models
{
    public class Hobby
    {
        [Key]
        // Idea ID --------------------------------------------------------------------------------------------
        public int HobbyId { get; set; }

        // Name -------------------------------------------------------------------------------------------
        [Required(ErrorMessage = "You must enter a Hobby Name")]
        [MinLength(2, ErrorMessage = "Your hobby name must be at least 2 characters long")]
        public string Name { get; set; }

        // Description -------------------------------------------------------------------------------------------
        [Required(ErrorMessage = "You must enter an Idea")]
        [MinLength(2, ErrorMessage = "Your hobby must be at least 2 characters long")]
        public string Description { get; set; }

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


