using System.ComponentModel.DataAnnotations;
using System;
namespace Project.Models
{
    public class Association
    {
        [Key]
        public int AssociationId { get; set; }
        public int UserId { get; set; }
        public int IdeaId { get; set; }
        public User User { get; set; }
        public Idea Idea { get; set; }
    }
}
