using System.ComponentModel.DataAnnotations;
using System;
namespace Project.Models
{
    public class Association
    {
        [Key]
        public int AssociationId { get; set; }
        public int id { get; set; }
        public int ProductId { get; set; }
        public User User { get; set; }
        public Wedding Wedding { get; set; }
    }
}
