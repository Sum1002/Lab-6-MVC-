using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookshopManagement.Models
{
    public class BookShop
    {
        [Key]
        public int BookShopId { get; set; }

        [Required(ErrorMessage = "Book is required")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Shop is required")]
        public int ShopId { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be non-negative")]
        public int Quantity { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Shop price must be greater than 0")]
        [DataType(DataType.Currency)]
        public decimal? ShopPrice { get; set; }

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string? Notes { get; set; }

        // Navigation properties
        [ForeignKey("BookId")]
        public virtual Book Book { get; set; } = null!;

        [ForeignKey("ShopId")]
        public virtual Shop Shop { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
