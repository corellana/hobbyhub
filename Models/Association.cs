using System.ComponentModel.DataAnnotations;
using System;
namespace Project.Models
{
    public class Association
    {
        [Key]
        public int AssociationId { get; set; }
        public int UserId { get; set; }
        public int CdActivityId { get; set; }
        public User User { get; set; }
        public CdActivity CdActivity { get; set; }
    }
}
