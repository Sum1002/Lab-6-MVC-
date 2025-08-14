using System.ComponentModel.DataAnnotations;

namespace BookshopManagement.Models
{
    public class Shop
    {
        [Key]
        public int ShopId { get; set; }

        [Required(ErrorMessage = "Shop name is required")]
        [StringLength(100, ErrorMessage = "Shop name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location is required")]
        [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters")]
        public string Location { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
        public string? Address { get; set; }

        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        public string? Phone { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string? Email { get; set; }

        [StringLength(100, ErrorMessage = "Website cannot exceed 100 characters")]
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string? Website { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Opening year must be non-negative")]
        public int? OpeningYear { get; set; }

        // Navigation properties
        public virtual ICollection<BookShop> BookShops { get; set; } = new List<BookShop>();
    }
}
