using System;
using System.ComponentModel.DataAnnotations;

namespace Web.DatabaseModel
{
    public class Product
    {
        [Key]
        public Guid ProductId { get; set; }

        [Required]
        public string Type { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public DateTime CreatedDate { get; set; }
        
        [Required]
        public bool IsActive { get; set; }
    }
}
