using System.ComponentModel.DataAnnotations;

namespace BookshopManagement.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        public string? Phone { get; set; }

        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string? Address { get; set; }

        [StringLength(100, ErrorMessage = "City cannot exceed 100 characters")]
        public string? City { get; set; }

        [StringLength(50, ErrorMessage = "State/Province cannot exceed 50 characters")]
        public string? State { get; set; }

        [StringLength(20, ErrorMessage = "Postal code cannot exceed 20 characters")]
        public string? PostalCode { get; set; }

        [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters")]
        public string? Country { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        // Computed property for full name
        public string FullName => $"{FirstName} {LastName}";

        // Navigation properties
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
