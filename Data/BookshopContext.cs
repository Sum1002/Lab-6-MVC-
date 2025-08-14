using BookshopManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace BookshopManagement.Data
{
    public class BookshopContext : DbContext
    {
        public BookshopContext(DbContextOptions<BookshopContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<BookShop> BookShops { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Book entity
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.BookId);
                entity.Property(e => e.ISBN).IsRequired();
                entity.HasIndex(e => e.ISBN).IsUnique();
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            });

            // Configure Shop entity
            modelBuilder.Entity<Shop>(entity =>
            {
                entity.HasKey(e => e.ShopId);
                entity.Property(e => e.Name).IsRequired();
            });

            // Configure Customer entity
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustomerId);
                entity.Property(e => e.Email).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configure BookShop junction table
            modelBuilder.Entity<BookShop>(entity =>
            {
                entity.HasKey(e => e.BookShopId);
                
                // Configure many-to-many relationship between Book and Shop
                entity.HasOne(e => e.Book)
                    .WithMany(e => e.BookShops)
                    .HasForeignKey(e => e.BookId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Shop)
                    .WithMany(e => e.BookShops)
                    .HasForeignKey(e => e.ShopId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.ShopPrice).HasColumnType("decimal(18,2)");

                // Ensure unique combination of Book and Shop
                entity.HasIndex(e => new { e.BookId, e.ShopId }).IsUnique();
            });

            // Configure Order entity
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId);
                
                // Configure relationship with Customer
                entity.HasOne(e => e.Customer)
                    .WithMany(e => e.Orders)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Configure relationship with BookShop
                entity.HasOne(e => e.BookShop)
                    .WithMany(e => e.Orders)
                    .HasForeignKey(e => e.BookShopId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.TotalPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ShippingCost).HasColumnType("decimal(18,2)");
                entity.Property(e => e.OrderDate).IsRequired();
                entity.Property(e => e.OrderStatus).IsRequired();
            });

            // Seed some initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Books
            modelBuilder.Entity<Book>().HasData(
                new Book { BookId = 1, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", ISBN = "978-0743273565", Price = 12.99m, PublicationYear = 1925, Genre = "Classic" },
                new Book { BookId = 2, Title = "To Kill a Mockingbird", Author = "Harper Lee", ISBN = "978-0446310789", Price = 14.99m, PublicationYear = 1960, Genre = "Classic" },
                new Book { BookId = 3, Title = "1984", Author = "George Orwell", ISBN = "978-0451524935", Price = 11.99m, PublicationYear = 1949, Genre = "Dystopian" }
            );

            // Seed Shops
            modelBuilder.Entity<Shop>().HasData(
                new Shop { ShopId = 1, Name = "Downtown Bookstore", Location = "Downtown", Address = "123 Main St", Phone = "555-0101", Email = "info@downtownbooks.com" },
                new Shop { ShopId = 2, Name = "University Bookshop", Location = "University District", Address = "456 College Ave", Phone = "555-0202", Email = "books@university.edu" },
                new Shop { ShopId = 3, Name = "Mall Bookstore", Location = "Shopping Mall", Address = "789 Mall Blvd", Phone = "555-0303", Email = "books@mall.com" }
            );

            // Seed Customers
            modelBuilder.Entity<Customer>().HasData(
                new Customer { CustomerId = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@email.com", Phone = "555-1001", City = "New York", State = "NY" },
                new Customer { CustomerId = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@email.com", Phone = "555-1002", City = "Los Angeles", State = "CA" },
                new Customer { CustomerId = 3, FirstName = "Bob", LastName = "Johnson", Email = "bob.johnson@email.com", Phone = "555-1003", City = "Chicago", State = "IL" }
            );

            // Seed BookShops
            modelBuilder.Entity<BookShop>().HasData(
                new BookShop { BookShopId = 1, BookId = 1, ShopId = 1, Quantity = 10, ShopPrice = 12.99m },
                new BookShop { BookShopId = 2, BookId = 1, ShopId = 2, Quantity = 5, ShopPrice = 13.99m },
                new BookShop { BookShopId = 3, BookId = 2, ShopId = 1, Quantity = 8, ShopPrice = 14.99m },
                new BookShop { BookShopId = 4, BookId = 2, ShopId = 3, Quantity = 12, ShopPrice = 15.99m },
                new BookShop { BookShopId = 5, BookId = 3, ShopId = 2, Quantity = 15, ShopPrice = 11.99m },
                new BookShop { BookShopId = 6, BookId = 3, ShopId = 3, Quantity = 7, ShopPrice = 12.99m }
            );

            // Seed Orders
            modelBuilder.Entity<Order>().HasData(
                new Order { OrderId = 1, CustomerId = 1, BookShopId = 1, Quantity = 2, OrderDate = DateTime.Now.AddDays(-5), TotalPrice = 25.98m, OrderStatus = "Completed" },
                new Order { OrderId = 2, CustomerId = 2, BookShopId = 3, Quantity = 1, OrderDate = DateTime.Now.AddDays(-3), TotalPrice = 14.99m, OrderStatus = "Shipped" },
                new Order { OrderId = 3, CustomerId = 3, BookShopId = 5, Quantity = 3, OrderDate = DateTime.Now.AddDays(-1), TotalPrice = 35.97m, OrderStatus = "Pending" }
            );
        }
    }
}
