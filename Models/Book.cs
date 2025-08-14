using System.ComponentModel.DataAnnotations;

namespace BookshopManagement.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Author is required")]
        [StringLength(100, ErrorMessage = "Author name cannot exceed 100 characters")]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "ISBN is required")]
        [RegularExpression(@"^(?:ISBN(?:-1[03])?:? )?(?=[0-9X]{10}$|(?=(?:[0-9]+[- ]){3})[- 0-9X]{13}$|97[89][0-9]{10}$|(?=(?:[0-9]+[- ]){4})[- 0-9X]{17}$)(?:97[89][- ]?)?[0-9]{1,5}[- ]?[0-9]+[- ]?[0-9]+[- ]?[0-9X]$", 
            ErrorMessage = "Please enter a valid ISBN")]
        public string ISBN { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string? Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Publication year must be non-negative")]
        public int? PublicationYear { get; set; }

        [StringLength(50, ErrorMessage = "Genre cannot exceed 50 characters")]
        public string? Genre { get; set; }

        // Navigation properties
        public virtual ICollection<BookShop> BookShops { get; set; } = new List<BookShop>();
    }
}
