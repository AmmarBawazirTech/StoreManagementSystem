using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StoreManagementSystem.Models
{
    public class Category
    {
      
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int CategoryId { get; set; }

            [Required(ErrorMessage = "Category name is required")]
            [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters")]
            [Display(Name = "Category Name")]
            public string Name { get; set; }

            [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
            [DataType(DataType.MultilineText)]
            public string? Description { get; set; }

            [Display(Name = "Active")]
            public bool IsActive { get; set; } = true;

            [Display(Name = "Display Order")]
            [Range(0, 100, ErrorMessage = "Display order must be between 0 and 100")]
            public int DisplayOrder { get; set; } = 0;

            [Display(Name = "Created Date")]
            [DataType(DataType.DateTime)]
            public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

            // Navigation property
            public virtual ICollection<Product>? Products { get; set; }
        }
    
}
