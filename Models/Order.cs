using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookshopManagement.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Customer is required")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "BookShop is required")]
        public int BookShopId { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Order quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Order date is required")]
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Total price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total price must be greater than 0")]
        [DataType(DataType.Currency)]
        public decimal TotalPrice { get; set; }

        [StringLength(50, ErrorMessage = "Order status cannot exceed 50 characters")]
        public string OrderStatus { get; set; } = "Pending";

        [StringLength(500, ErrorMessage = "Order notes cannot exceed 500 characters")]
        public string? OrderNotes { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ShippedDate { get; set; }

        [StringLength(100, ErrorMessage = "Shipping method cannot exceed 100 characters")]
        public string? ShippingMethod { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Shipping cost must be non-negative")]
        [DataType(DataType.Currency)]
        public decimal ShippingCost { get; set; } = 0;

        // Navigation properties
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; } = null!;

        [ForeignKey("BookShopId")]
        public virtual BookShop BookShop { get; set; } = null!;
    }
}
