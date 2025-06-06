using System.ComponentModel.DataAnnotations;

namespace Repositories.Models
{
    public class Product : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(1000)]
        public string? Description { get; set; }
        
        [Required]
        public decimal Price { get; set; }
        
        public int Stock { get; set; } = 0;
        
        public int CategoryId { get; set; }
        
        // Navigation property
        public virtual Category Category { get; set; } = null!;
    }
}
